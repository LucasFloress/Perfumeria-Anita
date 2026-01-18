using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Razor.Hosting;   

namespace PerfumeriaAnita.Models
{
    public class User
    {
        [Key]
        public int Id  {get; set;}

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
        public string Contraseña {get;set;} = null!;

        [Required]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email {get;set;} = null!;

        public string Rol {get;set;} = "Vendedor";

        /* ------------ Relaciones ---------------*/
        // Un usuario puede realizar muchas ventas
        public virtual ICollection<Venta> Ventas {get;set;} = new List<Venta>();
    }
}