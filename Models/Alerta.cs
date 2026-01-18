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
    
        /*------------ Relaciones ---------------*/
        // Saber qué producto generó la alerta
        public int? ProductoId {get;set;}
        [ForeignKey("ProductoId")]
        public virtual ICollection<Producto> Productos {get;set;}= new List<Producto>();
    }   
}
