using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Comercio.ViewModel.Categorias
{
    public class IdCategoriaViewModel
    {
        [Range(1, Int32.MaxValue, ErrorMessage = "Categoria é obrigatória.")]
        public int IdCategoria { get; set; }
    }
}