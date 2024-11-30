using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json; // Para deserializar o JSON retornado pela API ViaCEP
using System.Net.Http; // Para fazer requisições HTTP
using projetoihc.Models;

namespace projetoihc.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.Include(c => c.Endereco).ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Endereco)
                .FirstOrDefaultAsync(m => m.ClienteId == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Clientes cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }


        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.Include(c => c.Endereco).FirstOrDefaultAsync(c => c.ClienteId == id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Clientes cliente)
        {
            if (id != cliente.ClienteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.ClienteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Endereco)
                .FirstOrDefaultAsync(m => m.ClienteId == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.Include(c => c.Endereco).FirstOrDefaultAsync(c => c.ClienteId == id);
            if (cliente != null)
            {
                _context.Enderecos.Remove(cliente.Endereco);
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Método para buscar o endereço pelo CEP
        public async Task<IActionResult> PreencherEndereco(string cep)
        {
            if (string.IsNullOrEmpty(cep) || cep.Length != 9 || !cep.All(char.IsDigit))
            {
                return Json(null); // Retorna null caso o CEP seja inválido
            }

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync($"https://viacep.com.br/ws/{cep}/json/");

                    if (string.IsNullOrEmpty(response))
                    {
                        return Json(null); // Retorna null caso a resposta seja vazia
                    }

                    var endereco = JsonConvert.DeserializeObject<Endereco>(response);

                    if (endereco != null && !string.IsNullOrEmpty(endereco.Logradouro))
                    {
                        return Json(new
                        {
                            logradouro = endereco.Logradouro,
                            bairro = endereco.Bairro,
                            cidade = endereco.Cidade,
                            uf = endereco.UF,
                            complemento = endereco.Complemento
                        });
                    }
                }
            }
            catch (Exception)
            {
                return Json(null); // Retorna null em caso de erro de conexão ou outra falha
            }

            return Json(null); // Retorna null caso o endereço não seja encontrado ou erro
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ClienteId == id);
        }
    }
}
