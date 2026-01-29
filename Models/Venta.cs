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
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        // Opcional: si implementas usuarios
        public int? UsuarioId { get; set; }
        
        [StringLength(100)]
        public string? NombreVendedor { get; set; }

        // Relación: Una venta tiene muchos detalles
        public virtual ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
    }
}