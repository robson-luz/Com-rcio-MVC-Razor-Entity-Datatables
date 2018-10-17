using Comercio.Models.Produtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Comercio.Models.Categorias
{
    public class Categoria
    {
        public int IdCategoria { get; set; }

        public string Descricao { get; set; }

        public ICollection<Produto> Produtos { get; set; }
    }
}