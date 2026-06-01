using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Razor.Hosting;


namespace PerfumeriaAnita.Models
{   
    public class DetalleVenta
    {
        [Key]
        public int Id { get; set; }

        // Clave foránea a Venta
        [Required]
        public int VentaId { get; set; }
        
        [ForeignKey("VentaId")]
        public virtual Venta Venta { get; set; } = null!;

        // Clave foránea a Producto
        [Required]
        public int ProductoId { get; set; }
        
        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
        public int Cantidad { get; set; }

        // IMPORTANTE: Guardamos el precio al momento de la venta
        // Esto evita que cambios futuros de precio afecten el historial
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        // Campo calculado (no se guarda en BD)
        [NotMapped]
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}