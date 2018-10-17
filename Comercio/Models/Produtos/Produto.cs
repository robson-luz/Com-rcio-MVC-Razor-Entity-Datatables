using Comercio.Models.Categorias;
using Comercio.Models.Itens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Comercio.Models.Produtos
{
    public class Produto
    {
        public int IdProduto { get; set; }

        public int IdCategoria { get; set; }

        public string Descricao { get; set; }

        public decimal Preco { get; set; }


        public virtual Categoria Categoria { get; set; }

        public virtual ICollection<Item> Itens { get; set; }
    }
}