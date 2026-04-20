using System.ComponentModel.DataAnnotations;

namespace PerfumeriaAnita.Models
{
    public class AppConfig
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Clave { get; set; } = string.Empty;

        [Required]
        public string Valor { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Descripcion { get; set; }

        public DateTime UltimaModificacion { get; set; } = DateTime.Now;

        // Claves predefinidas como constantes para evitar typos
        public static class Keys
        {
            public const string NombreTienda     = "tienda.nombre";
            public const string EsloganTienda    = "tienda.eslogan";
            public const string ColorPrincipal   = "tienda.color_principal";
            public const string ZonaHoraria      = "sistema.zona_horaria";
            public const string FormatoHora      = "sistema.formato_hora";
            public const string EmailNotif       = "notif.email";
            public const string TelefonoNotif    = "notif.telefono";
            public const string NotifNuevoPedido = "notif.nuevo_pedido";
            public const string NotifStockBajo   = "notif.stock_bajo";
            public const string NotifTurno       = "notif.recordatorio_turno";
            public const string NotifResena      = "notif.resena";
            public const string NotifInforme     = "notif.informe_semanal";
            public const string NotifPush        = "notif.push_panel";
            public const string StockMinimoAlerta= "inventario.stock_minimo";
        }
    }
}
