using Comercio.Models.Pedidos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Comercio.ViewModel.Pedidos
{
    public class PedidoViewModel
    {
        public int Index { get; set; }

        public List<ItemViewModel> Itens { get; set; }

        public int IdPedido { get; set; }

        public List<Pedido> Pedidos { get; set; }
    }
}