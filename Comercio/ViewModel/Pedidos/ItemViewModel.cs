using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Comercio.ViewModel.Pedidos
{
    public class ItemViewModel
    {
        [Required(ErrorMessage = "Selecione o produto")]
        public int IdProduto { get; set; }

        public string Descricao { get; set; }

        public string Categoria { get; set; }

        [Required(ErrorMessage = "Campo {0} é obrigatório.")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "Campo {0} é obrigatório.")]
        public int Quantidade { get; set; }

    }
}