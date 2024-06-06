using Capa.Datos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Capa.Infraestructura.Persistencia
{
    public class CarritoDbContext : DbContext
    {
        public CarritoDbContext(DbContextOptions<CarritoDbContext> options) : base(options)
        {

        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<OrdenItem> OrdenItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cart>()
                 .HasMany(c => c.CartItems)
                 .WithOne(ci => ci.Cart)
                 .HasForeignKey(c => c.CartId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                 .HasOne(ci => ci.Producto)
                 .WithMany(p => p.CartItems)
                 .HasForeignKey(ci => ci.ProductId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrdenItem>()
                 .HasOne(oi => oi.Orden)
                 .WithMany(o => o.OrdenItems)
                 .HasForeignKey(oi => oi.OrdenId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrdenItem>()
                 .HasOne(oi => oi.Producto)
                 .WithMany(p => p.OrdenItems)
                 .HasForeignKey(oi => oi.ProductoId)
                 .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
