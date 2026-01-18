using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Razor.Hosting;


namespace PerfumeriaAnita.Models
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public decimal PrecioUnitario { get; set; }

        [Required]
        public DateTime FechaVenta { get; set; }

        /* ------------ Relaciones ---------------*/
        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; } = null!;
    
        // Relación con Usuario (Vendedor)
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; } // La clave foránea
        
        public virtual User? Usuario { get; set; } // La navegación
    }
}