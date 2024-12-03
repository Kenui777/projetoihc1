using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json; 
using System.Net.Http;
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

     
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.Include(c => c.Endereco).ToListAsync());
        }

       
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


        public IActionResult Create()
        {
            return View();
        }


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

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.Include(c => c.Endereco).FirstOrDefaultAsync(c => c.ClienteId == id);
            if (cliente != null)
            {
                if (cliente.Endereco != null)
                {
                    _context.Enderecos.Remove(cliente.Endereco);
                }

                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> PreencherEndereco(string cep)
        {
            if (string.IsNullOrEmpty(cep))
            {
                return Json(null);
            }

            cep = new string(cep.Where(char.IsDigit).ToArray());

            if (cep.Length != 8)
            {
                return Json(null);
            }

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync($"https://viacep.com.br/ws/{cep}/json/");
                    if (string.IsNullOrEmpty(response))
                    {
                        return Json(null);
                    }

                    var endereco = JsonConvert.DeserializeObject<Endereco>(response);
                    if (endereco != null && !string.IsNullOrEmpty(endereco.Logradouro))
                    {
                        return Json(new
                        {
                            logradouro = endereco.Logradouro,
                            bairro = endereco.Bairro,
                            localidade = endereco.Localidade, 
                            uf = endereco.UF,
                            complemento = endereco.Complemento
                        });
                    }
                }
            }
            catch (Exception)
            {
                return Json(null);
            }

            return Json(null);
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ClienteId == id);
        }
    }
}
