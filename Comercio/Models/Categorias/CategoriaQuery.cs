using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Comercio.Models.Categorias
{
    public static class CategoriaQuery
    {
        public static IQueryable<Categoria> ComId(this IQueryable<Categoria> categorias, int idCategoria)
        {
            if(idCategoria == 0)
            {
                return categorias;
            }

            return categorias.Where(c => c.IdCategoria == idCategoria);
        }

        public static IQueryable<Categoria> ComDescricao(this IQueryable<Categoria> categorias, string descricao)
        {
            if (String.IsNullOrEmpty(descricao))
            {
                return categorias;
            }

            return categorias.Where(c => c.Descricao == descricao);
        }

        public static IQueryable<Categoria> OndeDescricaoContem(this IQueryable<Categoria> categorias, string descricao)
        {
            if(String.IsNullOrEmpty(descricao))
            {
                return categorias;
            }

            return categorias.Where(c => c.Descricao.Contains(descricao));
        }
    }
}