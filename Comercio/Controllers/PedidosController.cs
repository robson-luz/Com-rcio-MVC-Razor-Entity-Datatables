using Comercio.Database;
using Comercio.Models.Itens;
using Comercio.Models.Pedidos;
using Comercio.Models.Produtos;
using Comercio.ViewModel.Pedidos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Comercio.Controllers
{
    public class PedidosController : Controller
    {
        DbComercio db;

        public PedidosController()
        {
            db = new DbComercio();
        }

        public ActionResult Index()
        {
            PedidoViewModel pedidoViewModel = new PedidoViewModel();

            return View(pedidoViewModel);
        }

        public ActionResult Adicionar(ItemViewModel viewModel)
        {
            if (viewModel.Quantidade <= 0)
                ModelState.AddModelError(String.Empty, "Quantidade inválida.");

            if (ModelState.IsValid)
            {
                ItemViewModel item = new ItemViewModel()
                {
                    IdProduto = viewModel.IdProduto,
                    Descricao = viewModel.Descricao,
                    Preco = viewModel.Preco,
                    Quantidade = viewModel.Quantidade
                };

                List<ItemViewModel> itensSession =(List<ItemViewModel>) this.Session["Itens"];
                if (itensSession == null)
                {
                    var itens = new List<ItemViewModel>();
                    itens.Add(item);

                    this.Session["Itens"] = itens;
                }
                else
                {
                    var mesmoItem = itensSession.Where(i => i.IdProduto == viewModel.IdProduto).SingleOrDefault();

                    if(mesmoItem == null)
                    {
                        itensSession.Add(item);
                    }
                    else
                    {
                        int index = itensSession.IndexOf(mesmoItem);

                        itensSession[index].Quantidade += viewModel.Quantidade;
                    }                    
                }

                return RedirectToAction("Index");
            }

            return View("AdicionarProduto", viewModel);
        }

        public ActionResult Remover(PedidoViewModel viewModel)
        {
            dynamic itens = this.Session["Itens"];
            itens.RemoveAt(viewModel.Index);

            if(itens.Count > 0)
            {
                viewModel.Itens = itens;
                this.Session["Itens"] = itens;
            } else
            {
                viewModel.Itens = null;
                this.Session["Itens"] = null;
            }

            return RedirectToAction("Finalizar", viewModel);
        }

        public ActionResult AdicionarProduto(int? id)
        {
            Produto produto = db.Produtos.Include(p => p.Categoria).ComId(id.Value).SingleOrDefault();

            ItemViewModel viewModel = new ItemViewModel()
            {
                IdProduto = produto.IdProduto,
                Descricao = produto.Descricao,
                Categoria = produto.Categoria.Descricao,
                Preco = produto.Preco,
                Quantidade = 1
            };

            return View(viewModel);
        }

        public ActionResult Finalizar()
        {
            var viewModel = new PedidoViewModel();
            viewModel.Itens = (dynamic)this.Session["Itens"];

            return View("Finalizar", viewModel);
        }

        [HttpPost]
        public ActionResult Finalizar(PedidoViewModel viewModel)
        {
            if (!viewModel.Itens.Any())
            {
                ModelState.AddModelError(String.Empty, "Adicione algum produto.");
            }

            if (ModelState.IsValid)
            {
                Pedido pedido = new Pedido()
                {
                    DataPedido = DateTime.Now
                };

                pedido.Itens = new List<Item>();

                foreach (ItemViewModel item in viewModel.Itens)
                {
                    var subtotalItem = item.Quantidade * item.Preco;

                    pedido.Itens.Add(new Item()
                    {
                        IdProduto = item.IdProduto,
                        Quantidade = item.Quantidade,
                        Subtotal = subtotalItem
                    });

                    pedido.ValorTotal += subtotalItem;
                }

                db.RegistrarNovo(pedido);

                db.Salvar();

                this.Session["Itens"] = null;

                TempData["Success"] = "Pedido finalizado com sucesso!";
                return RedirectToAction("Index", "Pedidos");
            }

            return View("Index", viewModel);
        }

        public ActionResult PedidosCadastrados()
        {
            PedidoViewModel viewModel = new PedidoViewModel();

            viewModel.Pedidos = db.Pedidos.ToList() ;

            return View("PedidosCadastrados", viewModel);
        }

        public ActionResult DetalhesPedido(int? id)
        {
            if (id != null)
            {
                Pedido pedido = db.
                    Pedidos
                    .Include(p => p.Itens.Select(i => i.Produto))
                    .Where(p => p.IdPedido == id.Value)
                    .SingleOrDefault();

                return View(pedido);
            }

            return  RedirectToAction("DetalhesPedido", "Pedidos"); ;
        }

    }
}