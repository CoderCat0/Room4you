using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Room4you.Data;
using Room4you.Models;
using static Room4you.Models.ViewModels;

namespace Room4you.Controllers
{
    [Authorize(Roles = "Administrador")] // esta 'anotação' garante que só as pessoas autenticadas têm acesso aos recursos
    public class FotografiasController : Controller
    {
        /// <summary>
        /// referência à base de dados
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

        public FotografiasController(Proj_Context context, IWebHostEnvironment caminho, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _caminho = caminho;
            _userManager = userManager;
        }

        /// <summary>
        /// Mostra uma lista de imagens dos hotéis
        /// </summary>
        [AllowAnonymous] // anula a necessidade de um utilizador estar autenticado
                         // para aceder a este método
        public async Task<IActionResult> Index()
        {

            /* criação de uma variável que vai conter um conjunto de dados
            * vindos da base de dados
            * se fosse em SQL, a pesquisa seria:
            *     SELECT *
            *     FROM Fotografias f, Hoteis h
            *     WHERE f.HotelFK = h.Id
            *  exatamente equivalente a _context.Fotografias.Include(f => f.Hoteis), feita em LINQ
            *  f => f.Hoteis  <---- expressão 'lambda'
            *  ^ ^  ^
            *  | |  |
            *  | |  representa cada um dos registos individuais da tabela das Fotografias
            *  | |  e associa a cada fotografia o seu respetivo hotel
            *  | |  equivalente à parte WHERE do comando SQL
            *  | |
            *  | um símbolo que separa os ramos da expressão
            *  |
            *  representa todos registos das fotografias
            */

            // dados de todas as fotografias
            var fotografias = await _context.Fotografias.Include(f => f.Hotel).ToListAsync();

            // quais as fotos que pertencem ao hotel?
            // quais os seus IDs?
            var hoteis = await (from h in _context.Hoteis
                              join f in _context.Fotografias on h.Id equals f.HotelFK
                              where f.HotelFK == h.Id
                              select f.Id)
                             .ToListAsync();

            var hotel = await _context.Hoteis.Include(h => h.ListaFotografias).ToListAsync();

            // transportar os dois objetos para a View
            // iremos usar um ViewModel
            //var fotos = new Hoteis
            //{
            //    ListaFotografias = fotografias,
            //    ListaFotosHoteis = hoteis
            //};

            // invoca a View, entregando-lhe a lista de registos das fotografias e hoteis
            return View(hotel);
        }

        // GET: Fotografias/Details/5
        /// <summary>
        /// Mostra os detalhes de uma fotografia
        /// </summary>
        /// <param name="id">Identificador da Fotografia</param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                // entro aqui se não foi especificado o ID
                // redirecionar para a página de início
                return RedirectToAction("Index");
            }
            // se chego aqui, foi especificado um ID
            // vou procurar se existe uma Fotografia com esse valor
            var fotografia = await _context.Fotografias
                                           .Include(f => f.Hotel)
                                           .FirstOrDefaultAsync(f => f.Id == id);

            if (fotografia == null)
            {
                // o ID especificado não corresponde a uma fotografia
                // redirecionar para a página de início
                return RedirectToAction("Index");
            }

            // se cheguei aqui, é pq a foto existe e foi encontrada
            // então, mostro-a na View
            return View(fotografia);
        }

        // GET: Fotografias/Create
        // [HttpGet]    não preciso desta definição, pois por omissão ele responde sempre em GET
        /// <summary>
        /// invoca, na primeira vez, a View com os dados de criação de uma fotografia
        /// </summary>
        public async Task<IActionResult> Create()
        {
            /* geração da lista de valores disponíveis na DropDown
             * o ViewData transporta dados a serem associados ao atributo 'HotelFK'
             * o SelectList é um tipo de dados especial que serve para armazenar a lista 
             * de opções de um objeto do tipo <SELECT> do HTML
             * Contém dois valores: ID + nome a ser apresentado no ecrã
             * 
             * _context.Hoteis : representa a fonte dos dados
             *                   na prática estamos a executar o comando SQL
             *                   SELECT * FROM Hoteis
             * 
             * vamos alterar a pesquisa para significar
             * SELECT * FROM Hoteis ORDER BY Nome
             * e, a minha expressão fica: _context.Hoteis.OrderBy(h=>h.Nome)
             * 
            */

            // _context.Hoteis.OrderBy(h => h.Nome)  -> obtem a lista de todos os hoteis
            var hoteis = await _context.Hoteis.OrderBy(h => h.Nome).Include(h => h.ListaFotografias).ToListAsync();

            ViewData["HotelFK"] = new SelectList(hoteis, "Id", "Nome");

            return View();
        }

        // POST: Fotografias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HotelFK")] Fotografias fotografias, IFormFile fotoHotel)
        {
            // avaliar se  o utilizador escolheu uma opção válida na dropdown do hotel
            if (fotografias.HotelFK < 0)
            {
                // não foi escolhido um hotel válido 
                ModelState.AddModelError("", "Escolha um hotel");

                // devolver o controlo à View
                var hoteis = await _context.Hoteis.OrderBy(h => h.Nome).Include(h => h.ListaFotografias).ToListAsync();

                ViewData["HotelFK"] = new SelectList(_context.Hoteis, "Id", "Id", fotografias.HotelFK);
                return View(fotografias);
            }


            /* processar o ficheiro
             *   - existe ficheiro?
             *     - se não existe, o q fazer?  => gerar uma msg erro, e devolver controlo à View
             *     - se continuo, é pq ficheiro existe
             *       - mas, será q é do tipo correto?
             *         - avaliar se é imagem,
             *           - se sim: - especificar o seu novo nome
             *                     - associar ao objeto 'foto', o nome deste ficheiro
             *                     - especificar a localização                     
             *                     - guardar ficheiro no disco rígido do servidor
             *           - se não  => gerar uma msg erro, e devolver controlo à View
            */

            // var auxiliar
            string nomeImagem = "";

            if (fotoHotel == null)
            {
                // não há ficheiro
                // adicionar msg de erro
                ModelState.AddModelError("", "Adicione uma foto de um hotel");
                // devolver o controlo à View
                var hoteis = await _context.Hoteis.OrderBy(h => h.Nome).Include(h => h.ListaFotografias).ToListAsync();

                ViewData["CaoFK"] = new SelectList(hoteis, "Id", "Nome");
                return View(fotografias);
            }
            else
            {
                // há ficheiro. Mas, será um ficheiro válido?
                // https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Basics_of_HTTP/MIME_types
                if (fotoHotel.ContentType == "image/jpeg" || fotoHotel.ContentType == "image/png")
                {
                    // definir o novo nome da fotografia     
                    Guid g;
                    g = Guid.NewGuid();
                    nomeImagem = fotografias.HotelFK + "_" + g.ToString(); // tb, poderia ser usado a formatação da data atual
                                                                  // determinar a extensão do nome da imagem
                    string extensao = Path.GetExtension(fotoHotel.FileName).ToLower();
                    // agora, consigo ter o nome final do ficheiro
                    nomeImagem = nomeImagem + extensao;

                    // associar este ficheiro aos dados da Fotografia do hotel
                    fotografias.Fotografia = nomeImagem;

                    // localização do armazenamento da imagem
                    string localizacaoFicheiro = _caminho.WebRootPath;
                    nomeImagem = Path.Combine(localizacaoFicheiro, "fotos", nomeImagem);
                }
                else
                {
                    // ficheiro não é válido
                    // adicionar msg de erro
                    ModelState.AddModelError("", "Só pode escolher uma imagem para a associar ao cão"); ////////////////////////////////////////
                    // devolver o controlo à View
                    var hoteis = await _context.Hoteis.OrderBy(h => h.Nome).Include(h => h.ListaFotografias).ToListAsync();

                    ViewData["HotelFK"] = new SelectList(hoteis, "Id", "Nome");
                    return View(fotografias);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    // adicionar os dados da nova fotografia à base de dados
                    _context.Add(fotografias);
                    // consolidar os dados na base de dados
                    await _context.SaveChangesAsync();

                    // se cheguei aqui, tudo correu bem
                    // vou guardar, agora, no disco rígido do Servidor a imagem
                    using var stream = new FileStream(nomeImagem, FileMode.Create);
                    await fotoHotel.CopyToAsync(stream);


                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocorreu um erro...");

                }
            }


            ViewData["HotelFK"] = new SelectList(_context.Hoteis.OrderBy(h => h.Nome), "Id", "Nome", fotografias.HotelFK);

            return View(fotografias);

        }
        
                // GET: Fotografias/Edit/5
                public async Task<IActionResult> Edit(int? id)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var fotografias = await _context.Fotografias.FindAsync(id);
                    if (fotografias == null)
                    {
                        return NotFound();
                    }
                    ViewData["HotelFK"] = new SelectList(_context.Hoteis, "Id", "Id", fotografias.HotelFK);

                    // guardar o ID do objeto enviado para o browser
                    // através de uma variável de sessão
                    HttpContext.Session.SetInt32("NumFotoEmEdicao", fotografias.Id);
                    //  Session["idFoto"] = fotografias.Id;


                    return View(fotografias);
                }

                // POST: Fotografias/Edit/5
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(int id, [Bind("Id,HotelFK")] Fotografias fotografias)
                {
                    if (id != fotografias.Id)
                    {
                        return NotFound();
                    }

                    // recuperar o ID do objeto enviado para o browser
                    var numIdFoto = HttpContext.Session.GetInt32("NumFotoEmEdicao");

                    // e compará-lo com o ID recebido
                    // se forem iguais, continuamos
                    // se forem diferentes, não fazemos a alteração

                    if (numIdFoto == null || numIdFoto != fotografias.Id)
                    {
                        // se entro aqui, é pq houve problemas

                        // redirecionar para a página de início
                        return RedirectToAction("Index");
                    }



                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(fotografias);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!FotografiasExists(fotografias.Id))
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
                    ViewData["CaoFK"] = new SelectList(_context.Hoteis, "Id", "Id", fotografias.HotelFK);
                    return View(fotografias);

                }

                // GET: Fotografias/Delete/5
                public async Task<IActionResult> Delete(int? id)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var fotografias = await _context.Fotografias
                        .Include(f => f.Hotel)
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (fotografias == null)
                    {
                        return NotFound();
                    }

                    return View(fotografias);
                }

                // POST: Fotografias/Delete/5
                [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> DeleteConfirmed(int id)
                {
                    var fotografias = await _context.Fotografias.FindAsync(id);
                    _context.Fotografias.Remove(fotografias);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                private bool FotografiasExists(int id)
                {
                    return _context.Fotografias.Any(e => e.Id == id);

                }
    }

}
