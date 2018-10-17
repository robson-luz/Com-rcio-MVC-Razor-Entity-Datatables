using Comercio.Database;
using Comercio.Models.Categorias;
using Comercio.ViewModel.Categorias;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Comercio.Controllers
{
    public class CategoriasController : Controller
    {
        DbComercio db;

        public CategoriasController()
        {
            db = new DbComercio();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cadastrar()
        {
            var viewModel = new CadastrarCategoriaViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Cadastrar(CadastrarCategoriaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Categoria categoriaBanco = db
                    .Categorias
                    .ComDescricao(viewModel.Descricao)
                    .SingleOrDefault();

                if (categoriaBanco != null)
                {
                    ModelState.AddModelError(String.Empty, "Já existe uma categoria com essa descrição.");
                    return View(viewModel);
                }

                Categoria categoria = new Categoria()
                {
                    Descricao = viewModel.Descricao
                };

                db.RegistrarNovo(categoria);

                db.Salvar();

                TempData["Success"] = "Categoria cadastrada com sucesso!";
                return RedirectToAction("Index", "Categorias");
            }

            return View(viewModel);
        }

        public ActionResult Atualizar(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Categoria categoria = db.Categorias.ComId(id.Value).SingleOrDefault();

            if (categoria == null)
                return HttpNotFound();

            AtualizarCategoriaViewModel viewModel = new AtualizarCategoriaViewModel()
            {
                IdCategoria = categoria.IdCategoria,
                Descricao = categoria.Descricao
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Atualizar(AtualizarCategoriaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Categoria categoria = db.Categorias.ComId(viewModel.IdCategoria).SingleOrDefault();

                Categoria categoriaBanco = db
                    .Categorias
                    .ComDescricao(viewModel.Descricao)
                    .Where(c => c.IdCategoria != categoria.IdCategoria)
                    .SingleOrDefault();

                if (categoriaBanco != null)
                {
                    ModelState.AddModelError(String.Empty, "Já existe uma categoria com essa descrição.");
                    return View(viewModel);
                }

                categoria.Descricao = viewModel.Descricao;

                db.RegistrarAlterado(categoria);

                db.Salvar();

                TempData["Success"] = "Categoria atualizada com sucesso!";
                return RedirectToAction("Index", "Categorias");
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Cadastradas()
        {
            //atributos da datatable
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("order[0][column]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            IQueryable<Categoria> categoriasQuery = db
                .Categorias
                .OndeDescricaoContem(searchValue);

            if (sortColumn == "1")
            {
                if (sortColumnDir == "asc")
                    categoriasQuery = categoriasQuery.OrderBy(c => c.Descricao);
                else if (sortColumnDir == "desc")
                    categoriasQuery = categoriasQuery.OrderByDescending(c => c.Descricao);
            }

            ICollection<Categoria> categorias = categoriasQuery
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            recordsTotal = categoriasQuery.ToList().Count;

            List<dynamic> categoriasJson = new List<dynamic>();

            foreach (Categoria categoria in categorias)
            {
                categoriasJson.Add(new
                {
                    idCategoria = categoria.IdCategoria,
                    descricao = categoria.Descricao
                });
            }

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = categoriasJson });
        }

        [HttpPost]
        public ActionResult Remover(IdCategoriaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Categoria categoria = db
                    .Categorias
                    .Include(c => c.Produtos)
                    .ComId(viewModel.IdCategoria)
                    .SingleOrDefault();

                if (categoria == null)
                {
                    return View("Index", viewModel);
                }
                if (categoria.Produtos.Any())
                {
                    return View("Index", viewModel);
                }

                db.RegistrarRemovido(categoria);

                db.SaveChanges();

                return Json(new { deletado = true });
            }

            return View("Index", viewModel);
        }
    }
}