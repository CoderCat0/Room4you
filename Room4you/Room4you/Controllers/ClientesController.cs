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
    public class ClientesController : Controller
    {
        /// <summary>
        /// referência à base de dados
        /// </summary>
        private readonly Proj_Context _context;

        /// <summary>
        /// objeto que sabe interagir com os dados do utilizador q se autentica
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;



        public ClientesController(Proj_Context context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

            var clientes = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientes == null)
            {
                return NotFound();
            }

            return View(clientes);
        }
/*
        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Nacionalidade,Nif,DataNasc,Sexo,UserName")] Clientes clientes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clientes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clientes);
        }
*/

        /// <summary>
        /// Método para apresentar os dados dos Clientes a autorizar
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ListaClientesPorAutorizar()
        {

            // quais os Clientes ainda não autorizados a aceder ao Sistema?
            // lista com os utilizadores bloqueados
            var listaDeUtilizadores = _userManager.Users.Where(u => u.LockoutEnd > DateTime.Now);
            // lista com os dados dos Criadores
            var listaCriadores = _context.Clientes
                                         .Where(c => listaDeUtilizadores.Select(u => u.Id)
                                                                       .Contains(c.UserName));
            /* Em SQL seria algo deste género
             * SELECT c.*
             * FROM Clientes c, Users u
             * WHERE c.UserName = u. Id AND
             *       u.LockoutEnd > Data Atual          * 
             */

            // Enviar os dados para a View
            return View(await listaCriadores.ToListAsync());
        }

        /// <summary>
        /// método que recebe os dados dos utilizadores a desbloquear
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Administrador")]
/*
         [Authorize(Roles = "Administrador")]  -->  só permite que pessoas com esta permissão entrem

         [Authorize(Roles = "Administrador,Cliente")]  --> permite acesso a pessoas com uma das duas roles

         [Authorize(Roles = "Administrador")]     -->
         [Authorize(Roles = "Cliente")]    -->  Neste caso, a pessoa tem de pertencer aos dois roles
*/
        public async Task<IActionResult> ListaClientesPorAutorizar(string[] clientes)
        {

            // será que algum cliente foi selecionado?
            if (clientes.Count() != 0)
            {
                // há clietes selecionados
                // para cada um, vamos desbloqueá-los
                foreach (string u in clientes)
                {
                    try
                    {
                        // procurar o 'user' na tabela dos Users
                        var user = await _userManager.FindByIdAsync(u);
                        // desbloquear o utilizador
                        await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddDays(-1));
                        // como não se pediu ao User para validar o seu email
                        // é preciso aqui validar esse email
                        string codigo = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        await _userManager.ConfirmEmailAsync(user, codigo);

                        // eventualmente, poderá ser enviado um email para o utilizador a avisar que 
                        // a sua conta foi desbloqueada
                    }
                    catch (Exception)
                    {
                        // deveria haver aqui uma mensagem de erro para o utilizador,
                        // se assim o entender
                    }
                }
            }

            return RedirectToAction("Index");
        }


        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientes = await _context.Clientes.FindAsync(id);
            if (clientes == null)
            {
                return NotFound();
            }
            return View(clientes);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Nacionalidade,Nif,DataNasc,Sexo,UserName")] Clientes clientes)
        {
            if (id != clientes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientesExists(clientes.Id))
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
            return View(clientes);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientes = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientes == null)
            {
                return NotFound();
            }

            return View(clientes);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientes = await _context.Clientes.FindAsync(id);
            _context.Clientes.Remove(clientes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientesExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
