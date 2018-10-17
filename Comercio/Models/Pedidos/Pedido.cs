using Comercio.Models.Itens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Comercio.Models.Pedidos
{
    public class Pedido
    {
        public int IdPedido { get; set; }

        public int IdCliente { get; set; }

        public DateTime DataPedido { get; set; }

        public decimal ValorTotal { get; set; }

        //public virtual Cliente Cliente { get; set; }

        public virtual ICollection<Item> Itens { get; set; }
    }
}