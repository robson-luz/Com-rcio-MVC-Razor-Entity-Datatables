using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Comercio.ViewModel.Categorias
{
    public class CadastrarCategoriaViewModel
    {
        [Required(ErrorMessage ="Campo {0} é obrigatório.")]
        [MaxLength(50, ErrorMessage = "Campo {0} deve ter no máximo {1} caracteres.")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

    }
}