using Comercio.Database.Mappers;
using Comercio.Models.Categorias;
using Comercio.Models.Itens;
using Comercio.Models.Pedidos;
using Comercio.Models.Produtos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Comercio.Database
{
    public class DbComercio : DbContext
    {
        public DbComercio() 
            : base("name=Comercio")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Item> Itens { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CategoriaMap());
            modelBuilder.Configurations.Add(new ItemMap());
            modelBuilder.Configurations.Add(new PedidoMap());
            modelBuilder.Configurations.Add(new ProdutoMap());
        }

        public void RegistrarNovo(object entidade)
        {
            Set(entidade.GetType()).Add(entidade);
        }

        public void RegistrarAlterado(object entidade)
        {
            Entry(entidade).State = EntityState.Modified;
        }

        public void RegistrarRemovido(object entidade)
        {
            Entry(entidade).State = EntityState.Deleted;
        }

        public void Salvar()
        {
            SaveChanges();
        }
    }
}