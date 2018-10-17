using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Comercio.ViewModel.Categorias
{
    public class AtualizarCategoriaViewModel
    {
        [Range(1,Int32.MaxValue, ErrorMessage="Categoria é obrigatória.")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage ="Campo {0} é obrigatório.")]
        [MaxLength(20, ErrorMessage = "Campo {0} deve ter no máximo {1} caracteres.")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
    }
}