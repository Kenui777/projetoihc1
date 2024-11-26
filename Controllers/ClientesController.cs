using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json; // Para deserializar o JSON retornado pela API ViaCEP
using System.Net.Http; // Para fazer requisições HTTP
using projetoihc.Models; // Adicione isso no início do arquivo

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
            return View(await _context.Clientes.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Endereco) // Incluindo o Endereço
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
        public async Task<IActionResult> Create([Bind("ClienteId,NomeCompleto,DataNascimento,RG,CPF,EstadoCivil,NomePai,NomeMae")] Clientes cliente, [Bind("Logradouro,Bairro,Localidade,Complemento,UF,CEP")] Endereco endereco)
        {
            if (ModelState.IsValid)
            {
                cliente.Endereco = endereco; // Associa o endereço ao cliente
                _context.Add(cliente);
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

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClienteId,NomeCompleto,DataNascimento,RG,CPF,Logradouro,Bairro,Localidade,Complemento,UF,CEP,EstadoCivil,NomePai,NomeMae")] Clientes cliente)
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
                .Include(c => c.Endereco) // Incluindo o Endereço
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
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ClienteId == id);
        }

        // Método para buscar o endereço pelo CEP
        [HttpGet]
        public async Task<IActionResult> PreencherEndereco(string cep)
        {
            if (string.IsNullOrEmpty(cep))
            {
                return BadRequest("CEP é obrigatório.");
            }

            try
            {
                using var client = new HttpClient();
                var response = await client.GetStringAsync($"https://viacep.com.br/ws/{cep}/json/");
                var endereco = JsonConvert.DeserializeObject<dynamic>(response);

                if (endereco.erro != null)
                {
                    return NotFound("CEP inválido ou não encontrado.");
                }

                // Retorna os dados do endereço
                return Json(new
                {
                    Logradouro = (string)endereco.logradouro,
                    Bairro = (string)endereco.bairro,
                    Cidade = (string)endereco.localidade,
                    UF = (string)endereco.uf
                });
            }
            catch (Exception ex)
            {
                // Caso ocorra um erro durante a requisição, retorna o erro
                return StatusCode(500, $"Erro ao buscar o CEP: {ex.Message}");
            }
        }
    }
}
