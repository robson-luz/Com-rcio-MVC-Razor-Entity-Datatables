using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Comercio.Models.Produtos;
using System.Data.Entity.ModelConfiguration;

namespace Comercio.Database.Mappers
{
    public class ProdutoMap : EntityTypeConfiguration<Produto>
    {
        public ProdutoMap()
        {
            ToTable("Produto", "dbo");

            HasKey(p => p.IdProduto);

            Property(p => p.IdProduto).IsRequired();
            Property(p => p.IdCategoria).IsRequired();
            Property(p => p.Descricao).HasMaxLength(50).IsRequired();
            Property(p => p.Preco).HasPrecision(11,2).IsRequired();

            HasRequired(p => p.Categoria).WithMany(c => c.Produtos).HasForeignKey(p => p.IdCategoria);
            HasMany(p => p.Itens).WithRequired(i => i.Produto).HasForeignKey(i => i.IdProduto);
        }
    }
}