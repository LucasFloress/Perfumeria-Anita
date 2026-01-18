using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Razor.Hosting;

namespace PerfumeriaAnita.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre es muy largo"),]
        public string Nombre { get; set; } = String.Empty;

        [Required]
        [Range(0.01, 999999, ErrorMessage = "El precio debe ser mayor a 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Required]
        [Range(0, 99, ErrorMessage = "El stock debe ser mayor a 0")]
        public int Stock { get; set; } = 0;

        public string? Categoria { get; set; }

        // Usamos byte[] que es compatible con ImputFile
        public byte[]? Imagen { get; set; }

        // Borrador logico (No borramos, solo lo ocultamos)
        public bool? Activo { get; set; } = true;

        /* ------------ Relaciones ---------------*/
        public virtual ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}