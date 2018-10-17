using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Comercio.Models.Itens;
using System.Data.Entity.ModelConfiguration;

namespace Comercio.Database.Mappers
{
    public class ItemMap : EntityTypeConfiguration<Item>
    {
        public ItemMap()
        {
            ToTable("Item", "dbo");

            HasKey(i => i.IdItem);

            Property(i => i.IdItem).IsRequired();
            Property(i => i.IdProduto).IsRequired();
            Property(i => i.IdPedido).IsRequired();
            Property(i => i.Quantidade).IsRequired();
            Property(i => i.Subtotal).HasPrecision(13,2).IsRequired();

            HasRequired(i => i.Produto).WithMany(p => p.Itens).HasForeignKey(i => i.IdProduto);
            HasRequired(i => i.Pedido).WithMany(p => p.Itens).HasForeignKey(i => i.IdPedido);
        }
    }
}