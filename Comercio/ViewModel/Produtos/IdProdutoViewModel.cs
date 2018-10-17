using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Comercio.ViewModel.Produtos
{
    public class IdProdutoViewModel
    {
        [Range(1, Int32.MaxValue, ErrorMessage = "Produto é obrigatório.")]
        public int IdProduto { get; set; }
    }
}