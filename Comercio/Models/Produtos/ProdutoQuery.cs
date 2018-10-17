using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Comercio.Models.Produtos
{
    public static class ProdutoQuery
    {
        public static IQueryable<Produto> ComId(this IQueryable<Produto> produtos, int idProduto)
        {
            if(idProduto == 0)
            {
                return produtos;
            }

            return produtos.Where(p => p.IdProduto == idProduto);
        }

        public static IQueryable<Produto> ComDescricao(this IQueryable<Produto> produtos, string descricao)
        {
            if (String.IsNullOrEmpty(descricao))
            {
                return produtos;
            }

            return produtos.Where(p => p.Descricao == descricao);
        }

        public static IQueryable<Produto> OndeDescricaoContem(this IQueryable<Produto> produtos, string descricao)
        {
            if(String.IsNullOrEmpty(descricao))
            {
                return produtos;
            }

            return produtos.Where(p => p.Descricao.Contains(descricao));
        }
    }
}