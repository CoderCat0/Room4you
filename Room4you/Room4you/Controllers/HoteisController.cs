using System;
using System.Collections.Generic;
using System.IO;
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
    public class HoteisController : Controller
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

        public HoteisController(Proj_Context context, IWebHostEnvironment caminho, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _caminho = caminho;
            _userManager = userManager;
        }

        // GET: Hoteis
        public async Task<IActionResult> Index()
        {
            // dados de todas as hoteis e respetivos quartos
            var dadosH = await _context.Hoteis.Include(h => h.ListaFotografias).Include(q => q.ListaQuartos).ToListAsync();

            return View(dadosH);
        }

        // GET: Hoteis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                // entro aqui se não foi especificado o ID
                // redirecionar para a página de início
                return RedirectToAction("Index", "Home"/*, new { area = "" }*/);
            }
            // se chego aqui, foi especificado um ID
            // vou procurar se existe uma Fotografia com esse valor
            var hoteis = await _context.Hoteis.Include(h => h.ListaQuartos).FirstOrDefaultAsync(m => m.Id == id);
            if (hoteis == null)
            {
                return NotFound();
            }

            return View(hoteis);
        }

        // GET: Hoteis/Create
        public IActionResult Create()
        {
            ViewData["QuartoFK"] = new SelectList(_context.Quartos, "Id", "Id");
            return View();
        }

        // POST: Hoteis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Pais,Cidade,Rua,Categoria,NumQuartos, Preco")] Hoteis hoteis, List<IFormFile> listaFotos, string area, string info, int numQuartos)
        {
            //flag erro
            bool flagErro = false;

            string nomeImagem = "";

            if (ModelState.IsValid)
            {
                List<Quartos> listaQuartosAUX = new List<Quartos>();

                for (int q = 1; q <= numQuartos; q++)
                {
                    //declarar variáveis
                    Quartos quartos = new Quartos();
                    quartos.Hotel = hoteis;
                    quartos.Comodidades = info;
                    quartos.Area = area;
                    listaQuartosAUX.Add(quartos);
                }

                List<string> listaNomesFotoFinal = new List<string>();

                if (listaFotos.Count == 0)
                {
                    //se não existe ficheiro
                    //adcicionar msg de erro
                    ModelState.AddModelError("", "Não foram adicionadas fotografias");
                    flagErro = true;
                }
                else
                {
                    //se a foto existe verificamos cada foto
                    foreach (IFormFile i in listaFotos)
                    {
                        //confirmamos se o tipo de ficheiro está certo
                        if (i.ContentType == "image/jpeg" || i.ContentType == "image/png")
                        {
                            Fotografias foto = new Fotografias();
                            //defenir novo nome da fotografia
                            Guid g;
                            g = Guid.NewGuid();
                            nomeImagem = hoteis.Id + "_" + g.ToString();

                            //determinar a extensão do nome da imagem
                            string extensao = Path.GetExtension(i.FileName).ToLower();

                            // agora, consigo ter o nome final do ficheiro
                            nomeImagem = nomeImagem + extensao;

                            // associar este ficheiro aos dados da Fotografia do hotel
                            foto.Nome = nomeImagem;
                            //foto.HotelFK = hoteis.Id;
                            hoteis.ListaFotografias.Add(foto);
                            string localizacaoFicheiro = _caminho.WebRootPath;
                            nomeImagem = Path.Combine(localizacaoFicheiro, "fotos", nomeImagem);

                            listaNomesFotoFinal.Add(nomeImagem);
                        }
                        else
                        {
                            //se foram adicionadas inválidos
                            //adcicionar msg de erro
                            ModelState.AddModelError("", "Os ficheiros adicionados não são válidos");
                            flagErro = true;

                        }
                    }
                }
                if (flagErro == false)
                    try
                    {
                        foreach(Quartos quartos in listaQuartosAUX)
                        {
                            hoteis.ListaQuartos.Add(quartos);
                        }

                        _context.Add(hoteis);

                        await _context.SaveChangesAsync();

                        for (int k = 0; k < listaFotos.Count; k++)
                        {
                            using var stream = new FileStream(listaNomesFotoFinal[k], FileMode.Create);
                            await listaFotos[k].CopyToAsync(stream);
                        }

                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.GetBaseException().ToString());
                    }
                else
                {
                    return View(hoteis);
                }
            }
            return View(hoteis);
        }

        // GET: Hoteis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Index");
            }

            var hotel = await _context.Hoteis
                .Include(h => h.ListaFotografias)
                .Where(h => h.Id == id)
                .FirstOrDefaultAsync();

            if (hotel == null)
            {
                return View("Index");
            }
            return View(hotel);
        }

        // POST: Hoteis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Pais,Cidade,Rua,Categoria,NumQuartos")] Hoteis hotel, int[] checkBoxFotos, List<IFormFile> addedListaFotos)
        {
            string nomeImagem = "";
            bool flagErro = false;

            if (id != hotel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                /*----------------------ALGORITMO
                 * se quero apagar imagens => quer dizer que checkBoxFotos contem ids de fotos
                 *    vai ser necessario desassociar as fotgrafias do hotel 
                 *    (ver ex que está no gitHub sobre editar M-N)
                 *    (não esquecer que as fotografias devem ser apagadas do disco)
                 *    
                 * se há fotos para adicionar => quer dizer que o fileUpload contem fotos
                 *    (fazer como no create) 
                 *    
                 * (não esquecer que no final do processo deve ficar pelo menos 1 foto associada so hotel)   
                 */

                ////dados anteriormente associados ao hotel
                //var dadosHotel = await _context.Hoteis.AsNoTracking()
                //                .Where(h => h.Id == id)
                //                .Include(h => h.ListaFotografias)
                //                .FirstOrDefaultAsync();

                ////lista dos IDs das fotografias associadas ao Hotel ANTES da edição
                //var oldListaFotos = dadosHotel.ListaFotografias
                //                                .Select(f => f.Id)
                //                                .ToList();

                //avaliar se o user alterou a lista de fotos associada ao hotel
                //adicionou -> lista de fotos a adicionar
      //          var added = addedListaFotos;
                //retiradas -> lista de fotos a manter
                // var removed = oldListaFotos.Except(checkBoxFotos.ToList());

                List<string> listaNomesFotoFinal = new List<string>();

                List<string> listaNomesFotoApagar = new List<string>();
                //se alguma foto foi adicionada ou removida
                //é necessário alterar a lista de fotos
                //associada ao Hotel
                //   if (added.Any() || removed.Any()) {
                if (checkBoxFotos.Any())
                {
                    //retirar a foto
                    foreach (int oldFoto in checkBoxFotos)
                    {
                        var fotoToRemove = await _context.Fotografias.FindAsync(oldFoto);
                        //  dadosHotel.ListaFotografias.Remove(fotoToRemove);
                        listaNomesFotoApagar.Add(new string(fotoToRemove.Nome));

                        _context.Fotografias.Remove(fotoToRemove);
                    }
                }

                if (addedListaFotos.Any())
                {
                    //adicionar foto
                    foreach (IFormFile newFoto in addedListaFotos)
                    {
                        if (newFoto.ContentType == "image/jpeg" || newFoto.ContentType == "image/png")
                        {
                            Fotografias foto = new Fotografias();
                            //defenir novo nome da fotografia
                            Guid g;
                            g = Guid.NewGuid();
                            nomeImagem = hotel.Id + "_" + g.ToString();

                            //determinar a extensão do nome da imagem
                            string extensao = Path.GetExtension(newFoto.FileName).ToLower();

                            // agora, consigo ter o nome final do ficheiro
                            nomeImagem = nomeImagem + extensao;

                            // associar este ficheiro aos dados da Fotografia do hotel
                            foto.Nome = nomeImagem;
                            hotel.ListaFotografias.Add(foto);
                            string localizacaoFicheiro = _caminho.WebRootPath;
                            nomeImagem = Path.Combine(localizacaoFicheiro, "fotos", nomeImagem);

                            listaNomesFotoFinal.Add(nomeImagem);
                        }
                        else
                        {
                            //se foram adicionadas ficheiros inválidos
                            //adcicionar msg de erro
                            ModelState.AddModelError("", "Os ficheiros adicionados não são válidos");
                            flagErro = true;

                        }

                        //var fotoToAdd = await _context.Fotografias.FirstOrDefaultAsync(f => f.Id == newFoto);
                        //dadosHotel.ListaFotografias.Add(fotoToAdd);
                    }
                }
                // }


                if (flagErro == false)    //!flagErro
                {
                    try
                    {
                        ////atualizar com os novos dados provenientes da view
                        //dadosHotel.Nome = hotel.Nome;
                        //dadosHotel.Pais = hotel.Pais;
                        //dadosHotel.Rua = hotel.Rua;
                        //dadosHotel.NumQuartos = hotel.NumQuartos;

                        //_context.Update(dadosHotel);
                        _context.Update(hotel);
                        await _context.SaveChangesAsync();
                        //a remocao so deve ser feita após esta fase

                        foreach (string nomeFoto in listaNomesFotoApagar)
                        {
                            //File.Delete(Path.Combine(rootFolder, nomeFoto));
                            var path = Path.Combine(_caminho.WebRootPath, "fotos", nomeFoto);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                        }

                        for (int k = 0; k < addedListaFotos.Count; k++)
                        {
                            using var stream = new FileStream(listaNomesFotoFinal[k], FileMode.Create);
                            await addedListaFotos[k].CopyToAsync(stream);
                        }

                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.GetBaseException().ToString());
                        //verifica se existe algum hotel com este id
                        if (_context.Hoteis.Any(o => o.Id == hotel.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // GET: Hoteis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoteis = await _context.Hoteis.Include(h => h.ListaQuartos).FirstOrDefaultAsync(m => m.Id == id);

            if (hoteis == null)
            {
                return NotFound();
            }

            return View(hoteis);
        }

        // POST: Hoteis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hotel = await _context.Hoteis.FindAsync(id);
            _context.Hoteis.Remove(hotel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoteisExists(int id)
        {
            return _context.Hoteis.Any(e => e.Id == id);
        }

    }

}
