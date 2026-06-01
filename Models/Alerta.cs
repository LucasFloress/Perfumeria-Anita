using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Razor.Hosting;

namespace PerfumeriaAnita.Models
{
    public class Alerta
    {
        [Key]
        public int Id {get;set;}
        [Required]
        public string Nombre {get;set;} = string.Empty;

        [Required]
        public string Mensaje {get;set;} = string.Empty;

        [Required]
        public DateTime Fecha {get;set;} = DateTime.Now;
    
        [StringLength(50)]
        public string Tipo { get; set; } = "Info"; // "Info", "Warning", "Error"

        public bool Leida {get;set;} = true;


        /*------------ Relaciones ---------------*/
        // Saber qué producto generó la alerta
        public int? ProductoId {get;set;}
        [ForeignKey("ProductoId")]
        public virtual Producto? Producto {get;set;}
    }   
}
