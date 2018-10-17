using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Comercio.ViewModel.Produtos
{
    public class AtualizarProdutoViewModel
    {
        [Range(1,Int32.MaxValue, ErrorMessage="Produto é obrigatória.")]
        public int IdProduto { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Campo {0} é obrigatório.")]
        [Display(Name = "Categoria")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "Campo {0} é obrigatório.")]
        [Display(Name = "Preço")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage ="Campo {0} é obrigatório.")]
        [MaxLength(50, ErrorMessage = "Campo {0} deve ter no máximo {1} caracteres.")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
    }
}