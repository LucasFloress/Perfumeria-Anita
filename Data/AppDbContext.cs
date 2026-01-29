using Microsoft.EntityFrameworkCore;
using PerfumeriaAnita.Models;


namespace PerfumeriaAnita.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // --------------- Tablas ------------------
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<User> Usuarios {get;set;}
        public DbSet<Alerta> Alertas {get;set;}
        public DbSet<Categoria> Categorias {get;set;}
        public DbSet<DetalleVenta> DetalleVentas {get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de DetalleVenta
            modelBuilder.Entity<DetalleVenta>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.PrecioUnitario)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                // Relación con Venta (Obligatoria)
                entity.HasOne(d => d.Venta)
                    .WithMany(v => v.Detalles)
                    .HasForeignKey(d => d.VentaId)
                    .OnDelete(DeleteBehavior.Cascade); // Si se borra venta, se borran detalles

                // Relación con Producto (Obligatoria)
                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.DetallesVenta)  // ⭐ Aquí usa la propiedad que agregamos
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.Restrict); // NO se puede borrar producto con ventas
            });

            // Configuración de Productos
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CodigoBarras).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.CodigoBarras).IsUnique(); // Código único
            });

            // Configuración de Ventas
            modelBuilder.Entity<Venta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Total)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            });

            // Configuración de Categorías (si usás tabla)
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Nombre).IsUnique();
            });
        }
    }
}