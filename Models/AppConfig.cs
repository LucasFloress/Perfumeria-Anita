using System.ComponentModel.DataAnnotations;

namespace PerfumeriaAnita.Models
{
    public class AppConfig
    {
        [Key]
        public int      Id                 { get; set; }
        
        [Required]
        [StringLength(100)]
        public string   Clave              { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string   Valor              { get; set; } = string.Empty;

        [StringLength(255)]
        public string?  Descripcion        { get; set; }
        public DateTime UltimaModificacion { get; set; } = DateTime.Now;

        /// <summary>
        /// Constantes de las claves de configuración
        /// </summary>
        public static class Keys
        {
            // Apariencia
            public const string NombreTienda      = "tienda.nombre";
            public const string EsloganTienda     = "tienda.eslogan";
            public const string ColorPrincipal    = "tienda.color_principal";
            public const string LogoBase64        = "tienda.logo_base64";

            // Sistema
            public const string ZonaHoraria       = "sistema.zona_horaria";
            public const string FormatoHora       = "sistema.formato_hora";

            // Notificaciones
            public const string EmailNotif        = "notif.email";
            public const string TelefonoNotif     = "notif.telefono";
            public const string NotifNuevoPedido  = "notif.nuevo_pedido";
            public const string NotifStockBajo    = "notif.stock_bajo";
            public const string NotifTurno        = "notif.recordatorio_turno";
            public const string NotifResena       = "notif.resena";
            public const string NotifInforme      = "notif.informe_semanal";
            public const string NotifPush         = "notif.push_panel";

            // Inventario
            public const string StockMinimoAlerta = "inventario.stock_minimo";

            // Venta
            public const string PorcentajeAumento  = "venta.porcentaje_aumento";
            public const string MetodoPagoDefault  = "venta.metodo_pago_default";
        }
    }
}