using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Room4you.Data;
using Room4you.Models;

namespace Room4you.Controllers
{
    [Authorize(Roles = "Administrador")] 
    [Authorize(Roles = "Cliente")] // estas 'anotações' garante que só as pessoas autenticadas têm acesso aos recursos
    public class QuartosController : Controller
    {
        /// <summary>
        /// este atributo representa a base de dados do projeto
        /// </summary>

        private readonly Proj_Context _context;

        /// <summary>
        /// objeto que sabe interagir com os dados do utilizador q se autentica
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;


        public QuartosController(Proj_Context context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Quartos
        public async Task<IActionResult> Index()
        {
            //dados de todos os quartos
            var quartos = await _context.Quartos.Include(q => q.Quarto).ToListAsync();

            // var. auxiliar
            string idDaPessoaAutenticada = _userManager.GetUserId(User);

            // quais os quartos que pertencem à pessoa que está autenticada?
            // quais os seus IDs?
            var quartosRes = await (from q in _context.Quartos
                                    join qc in _context.QuartosCompra on q.Id equals qc.IdQuartoFK
                                    join c in _context.Compras on qc.IdCompraFK equals c.Id
                                    join cli in _context.Clientes on c.Id equals cli.CompraFK
                                    where cli.UserName == idDaPessoaAutenticada
                                    select q.Id)
                                    .ToListAsync();

            // transportar os dois objetos para a View
            // iremos usar um ViewModel
            var fotos = new FotosCaes
            {
                ListaCaes = caes,
                ListaFotografias = fotografias
            };


        }

        // GET: Quartos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quartos = await _context.Quartos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quartos == null)
            {
                return NotFound();
            }

            return View(quartos);
        }

        // GET: Quartos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Quartos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Area,Comodidades")] Quartos quartos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quartos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quartos);
        }

        // GET: Quartos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quartos = await _context.Quartos.FindAsync(id);
            if (quartos == null)
            {
                return NotFound();
            }
            return View(quartos);
        }

        // POST: Quartos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Area,Comodidades")] Quartos quartos)
        {
            if (id != quartos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quartos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuartosExists(quartos.Id))
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
            return View(quartos);
        }

        // GET: Quartos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quartos = await _context.Quartos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quartos == null)
            {
                return NotFound();
            }

            return View(quartos);
        }

        // POST: Quartos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quartos = await _context.Quartos.FindAsync(id);
            _context.Quartos.Remove(quartos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuartosExists(int id)
        {
            return _context.Quartos.Any(e => e.Id == id);
        }
    }
}
