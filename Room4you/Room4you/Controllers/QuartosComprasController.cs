using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Room4you.Data;
using Room4you.Models;

namespace Room4you.Controllers
{
    public class QuartosComprasController : Controller
    {
        /// <summary>
        /// este atributo representa a base de dados do projeto
        /// </summary>
        private readonly Proj_Context _context;

        /// <summary>
        /// este atributo contém os dados da app web no servidor
        /// </summary>
        private readonly IWebHostEnvironment _caminho;

        /// <summary>
        /// esta variável recolhe os dados da pessoa q se autenticou
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        public QuartosComprasController(Proj_Context context, IWebHostEnvironment caminho, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _caminho = caminho;
            _userManager = userManager;
        }

        // GET: QuartosCompras
        public async Task<IActionResult> Index()
        {
            var proj_Context = await _context.QuartosCompra.Include(q => q.IdCompra).Include(q => q.IdQuarto).ToListAsync();
            return View(proj_Context);
        }

        // GET: QuartosCompras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quartosCompra = await _context.QuartosCompra
                .Include(q => q.IdCompra)
                .Include(q => q.IdQuarto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quartosCompra == null)
            {
                return NotFound();
            }

            return View(quartosCompra);
        }

        // GET: QuartosCompras/Create
        public IActionResult Create()
        {
            ViewData["IdCompraFK"] = new SelectList(_context.Compras, "Id", "Id");
            ViewData["IdQuartoFK"] = new SelectList(_context.Quartos, "Id", "Area");
            return View();
        }

        // POST: QuartosCompras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DataEntrada,DataSaida,NumPessoas,Preco,IdQuartoFK,IdCompraFK")] QuartosCompra quartosCompra, Hoteis hotel)
        {
            //flag erro
            bool flagErro = false;
            int contaQuartos = 0;

            Random r = new Random();
            int preco = r.Next(100, 150);
            ViewBag.Message = preco;


            if (ModelState.IsValid)
            {
                Quartos disponivel = null;

                foreach (Quartos quarto in hotel.ListaQuartos)
                {
                    if (quarto.Ocupado == false)
                    {
                        if(disponivel == null)
                        {
                            disponivel = quarto;
                        }
                        contaQuartos++;
                    }
                }


                if(contaQuartos < 1)
                {
                    ModelState.AddModelError("", "Não estão quartos desocupados para o hotel escolhido");
                }
                else
                {
                    disponivel.Ocupado = true;
                    QuartosCompra quartoReservado = new QuartosCompra();
                    quartoReservado.IdQuartoFK = disponivel.Id;
                    quartoReservado.IdCompraFK = quartosCompra.Id;
                    quartoReservado.DataEntrada = quartosCompra.DataEntrada;
                    quartoReservado.DataSaida = quartosCompra.DataSaida;
                    quartoReservado.NumPessoas = quartosCompra.NumPessoas;
                    quartoReservado.Preco = preco;
                    
                }
                _context.Add(quartosCompra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCompraFK"] = new SelectList(_context.Compras, "Id", "Id", quartosCompra.IdCompraFK);
            ViewData["IdQuartoFK"] = new SelectList(_context.Quartos, "Id", "Area", quartosCompra.IdQuartoFK);
            return View(quartosCompra);
        }

        // GET: QuartosCompras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quartosCompra = await _context.QuartosCompra.FindAsync(id);
            if (quartosCompra == null)
            {
                return NotFound();
            }
            ViewData["IdCompraFK"] = new SelectList(_context.Compras, "Id", "Id", quartosCompra.IdCompraFK);
            ViewData["IdQuartoFK"] = new SelectList(_context.Quartos, "Id", "Area", quartosCompra.IdQuartoFK);
            return View(quartosCompra);
        }

        // POST: QuartosCompras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DataEntrada,DataSaida,NumPessoas,Preco,IdQuartoFK,IdCompraFK")] QuartosCompra quartosCompra)
        {
            if (id != quartosCompra.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quartosCompra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuartosCompraExists(quartosCompra.Id))
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
            ViewData["IdCompraFK"] = new SelectList(_context.Compras, "Id", "Id", quartosCompra.IdCompraFK);
            ViewData["IdQuartoFK"] = new SelectList(_context.Quartos, "Id", "Area", quartosCompra.IdQuartoFK);
            return View(quartosCompra);
        }

        // GET: QuartosCompras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quartosCompra = await _context.QuartosCompra
                .Include(q => q.IdCompra)
                .Include(q => q.IdQuarto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quartosCompra == null)
            {
                return NotFound();
            }

            return View(quartosCompra);
        }

        // POST: QuartosCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quartosCompra = await _context.QuartosCompra.FindAsync(id);
            _context.QuartosCompra.Remove(quartosCompra);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuartosCompraExists(int id)
        {
            return _context.QuartosCompra.Any(e => e.Id == id);
        }
    }
}
