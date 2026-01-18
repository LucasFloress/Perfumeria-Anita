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
    }
}