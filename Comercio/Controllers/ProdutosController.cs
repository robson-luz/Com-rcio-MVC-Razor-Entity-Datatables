using Comercio.Database;
using Comercio.Models.Produtos;
using Comercio.ViewModel.Produtos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Comercio.Controllers
{
    public class ProdutosController : Controller
    {
        DbComercio db;

        public ProdutosController()
        {
            db = new DbComercio();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cadastrar()
        {
            ViewBag.Categorias = db.Categorias;

            var viewModel = new CadastrarProdutoViewModel();
            return View(viewModel);

        }

        [HttpPost]
        public ActionResult Cadastrar(CadastrarProdutoViewModel viewModel)
        {
            if(viewModel.Preco <= 0)
            {
                ModelState.AddModelError(String.Empty, "Preço inválido.");
            }

            if (ModelState.IsValid)
            {
                Produto produtoBanco = db
                    .Produtos
                    .ComDescricao(viewModel.Descricao)
                    .SingleOrDefault();

                if (produtoBanco != null)
                {
                    ModelState.AddModelError(String.Empty, "Já existe um produto com essa descrição.");
                    return View("Cadastrar", viewModel);
                }

                Produto produto = new Produto()
                {
                    IdCategoria = viewModel.IdCategoria,
                    Descricao = viewModel.Descricao,
                    Preco = viewModel.Preco
                };

                db.RegistrarNovo(produto);

                db.Salvar();

                TempData["Success"] = "Produto cadastrado com sucesso!";
                return RedirectToAction("Index", "Produtos");
            }

            ViewBag.Categorias = db.Categorias;
            return View(viewModel);
        }

        public ActionResult Atualizar(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Produto produto = db.Produtos.ComId(id.Value).SingleOrDefault();

            if (produto == null)
                return HttpNotFound();

            ViewBag.Categorias = db.Categorias;
            AtualizarProdutoViewModel viewModel = new AtualizarProdutoViewModel()
            {
                IdProduto = produto.IdProduto,
                IdCategoria = produto.IdCategoria,
                Descricao = produto.Descricao,
                Preco = produto.Preco
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Atualizar(AtualizarProdutoViewModel viewModel)
        {
            if (viewModel.Preco <= 0)
            {
                ModelState.AddModelError(String.Empty, "Preço inválido.");
            }

            if (ModelState.IsValid)
            {
                Produto produto = db.Produtos.ComId(viewModel.IdProduto).SingleOrDefault();

                Produto produtoBanco = db
                    .Produtos
                    .ComDescricao(viewModel.Descricao)
                    .Where(p => p.IdProduto != produto.IdProduto)
                    .SingleOrDefault();

                if (produtoBanco != null)
                {
                    ModelState.AddModelError(String.Empty, "Já existe um produto com essa descrição.");
                    return View("Atualizar", viewModel);
                }

                produto.IdCategoria = viewModel.IdCategoria;
                produto.Descricao = viewModel.Descricao;
                produto.Preco = viewModel.Preco;

                db.RegistrarAlterado(produto);

                db.Salvar();

                TempData["Success"] = "Produto atualizado com sucesso!";
                return RedirectToAction("Index", "Produtos");
            }

            ViewBag.Categorias = db.Categorias;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Cadastrados()
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

            IQueryable<Produto> produtosQuery = db
                .Produtos
                .Include(p => p.Categoria)
                .OndeDescricaoContem(searchValue);

            if (sortColumn == "1")
            {
                if (sortColumnDir == "asc")
                    produtosQuery = produtosQuery.OrderBy(p => p.Descricao);
                else if (sortColumnDir == "desc")
                    produtosQuery = produtosQuery.OrderByDescending(p => p.Descricao);
            }

            ICollection<Produto> produtos = produtosQuery
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            recordsTotal = produtosQuery.ToList().Count;

            List<dynamic> produtosJson = new List<dynamic>();

            foreach (Produto produto in produtos)
            {
                produtosJson.Add(new
                {
                    idProduto = produto.IdProduto,
                    idCategoria = produto.IdCategoria,
                    categoria = produto.Categoria.Descricao,
                    descricao = produto.Descricao,
                    preco = produto.Preco
                });
            }

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = produtosJson });
        }

        [HttpPost]
        public ActionResult Remover(IdProdutoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Produto produto = db
                    .Produtos
                    .Include(p => p.Itens)
                    .ComId(viewModel.IdProduto)
                    .SingleOrDefault();

                if (produto == null)
                {
                    return View("Index", viewModel);
                }
                if (produto.Itens.Any())
                {
                    return View("Index", viewModel);
                }

                db.RegistrarRemovido(produto);

                db.SaveChanges();

                return Json(new { deletado = true });
            }

            return View("Index", viewModel);
        }
    }
}