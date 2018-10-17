using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Comercio.Models.Pedidos;
using System.Data.Entity.ModelConfiguration;

namespace Comercio.Database.Mappers
{
    public class PedidoMap : EntityTypeConfiguration<Pedido>
    {
        public PedidoMap()
        {
            ToTable("Pedido", "dbo");

            HasKey(p => p.IdPedido);

            Property(p => p.IdPedido).IsRequired();
            Property(p => p.IdCliente).IsOptional();
            Property(p => p.DataPedido).IsRequired();
            Property(p => p.ValorTotal).HasPrecision(13,2).IsRequired();

            //HasRequired(p => p.Cliente).WithMany(c => c.Pedidos).HasForeignKey(p => p.IdCliente);
            HasMany(p => p.Itens).WithRequired(i => i.Pedido).HasForeignKey(i => i.IdPedido);
        }
    }
}