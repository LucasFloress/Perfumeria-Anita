using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace PerfumeriaAnita.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre es muy largo")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 999999, ErrorMessage = "El precio debe ser mayor a 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Required]
        [Range(0, 9999, ErrorMessage = "El stock debe ser mayor a 0")] // Aumenté el stock máx a 9999
        public int Stock { get; set; } = 0;

        [Required(ErrorMessage = "El codigo de barras es obligatorio")]
        [StringLength(50)]
        public string? CodigoBarras { get; set; }

        // --- Relación con Categoría ---
        public int? CategoriaId { get; set; }
        
        [ForeignKey("CategoriaId")]
        public virtual Categoria? Categoria { get; set; }

        public byte[]? Imagen { get; set; }

        public bool? Activo { get; set; } = true;

        /* ------------ Relaciones ---------------*/
        // Solo dejamos la relación con los detalles (Así es correcto)
        public virtual ICollection<DetalleVenta>? DetallesVenta { get; set; } = new List<DetalleVenta>();
        // End of class Producto
    }
}