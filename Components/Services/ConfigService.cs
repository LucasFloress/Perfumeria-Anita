using Microsoft.EntityFrameworkCore;
using PerfumeriaAnita.Data;
using PerfumeriaAnita.Models;

namespace PerfumeriaAnita.Services
{
    public interface IConfigService
    {
        Task<string?> GetAsync(string clave);
        Task<string> GetAsync(string clave, string valorDefault);
        Task SetAsync(string clave, string valor, string? descripcion = null);
        Task<Dictionary<string, string>> GetAllAsync();
        Task SeedDefaultsAsync();
    }

    public class ConfigService : IConfigService
    {
        private readonly AppDbContext _db;

        public ConfigService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<string?> GetAsync(string clave)
        {
            var config = await _db.AppConfigs
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Clave == clave);
            return config?.Valor;
        }

        public async Task<string> GetAsync(string clave, string valorDefault)
        {
            return await GetAsync(clave) ?? valorDefault;
        }

        public async Task SetAsync(string clave, string valor, string? descripcion = null)
        {
            var existing = await _db.AppConfigs.FirstOrDefaultAsync(c => c.Clave == clave);

            if (existing is null)
            {
                _db.AppConfigs.Add(new AppConfig
                {
                    Clave              = clave,
                    Valor              = valor,
                    Descripcion        = descripcion,
                    UltimaModificacion = DateTime.Now
                });
            }
            else
            {
                existing.Valor              = valor;
                existing.UltimaModificacion = DateTime.Now;
                if (descripcion is not null) existing.Descripcion = descripcion;
            }

            await _db.SaveChangesAsync();
        }

        public async Task<Dictionary<string, string>> GetAllAsync()
        {
            // ToList primero para evitar que ToDictionaryAsync explote con claves duplicadas
            var configs = await _db.AppConfigs
                .AsNoTracking()
                .ToListAsync();

            // GroupBy por si hubiera duplicados: nos quedamos con el registro más reciente
            return configs
                .GroupBy(c => c.Clave)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderByDescending(c => c.Id).First().Valor
                );
        }

        public async Task SeedDefaultsAsync()
        {
            if (await _db.AppConfigs.AnyAsync()) return;

            var defaults = new List<AppConfig>
            {
                new() { Clave = AppConfig.Keys.NombreTienda,      Valor = "Anita",                          Descripcion = "Nombre de la perfumería" },
                new() { Clave = AppConfig.Keys.EsloganTienda,     Valor = "Tu perfume, tu identidad",       Descripcion = "Eslogan visible en la interfaz" },
                new() { Clave = AppConfig.Keys.ColorPrincipal,    Valor = "#D4537E",                        Descripcion = "Color principal (hex)" },
                new() { Clave = AppConfig.Keys.ZonaHoraria,       Valor = "America/Argentina/Buenos_Aires", Descripcion = "Zona horaria del sistema" },
                new() { Clave = AppConfig.Keys.FormatoHora,       Valor = "24",                             Descripcion = "Formato de hora: 12 o 24" },
                new() { Clave = AppConfig.Keys.EmailNotif,        Valor = "anita@perfumeria.com",           Descripcion = "Email receptor de alertas" },
                new() { Clave = AppConfig.Keys.TelefonoNotif,     Valor = "",                               Descripcion = "Teléfono/WhatsApp para alertas urgentes" },
                new() { Clave = AppConfig.Keys.NotifNuevoPedido,  Valor = "true",                           Descripcion = "Notificar nuevo pedido" },
                new() { Clave = AppConfig.Keys.NotifStockBajo,    Valor = "true",                           Descripcion = "Notificar stock bajo" },
                new() { Clave = AppConfig.Keys.NotifTurno,        Valor = "true",                           Descripcion = "Recordatorio de turno 24hs antes" },
                new() { Clave = AppConfig.Keys.NotifResena,       Valor = "false",                          Descripcion = "Notificar nueva reseña" },
                new() { Clave = AppConfig.Keys.NotifInforme,      Valor = "true",                           Descripcion = "Informe semanal de ventas" },
                new() { Clave = AppConfig.Keys.NotifPush,         Valor = "true",                           Descripcion = "Notificaciones push en el panel" },
                new() { Clave = AppConfig.Keys.StockMinimoAlerta, Valor = "5",                              Descripcion = "Cantidad mínima para alerta de stock" },
            };

            _db.AppConfigs.AddRange(defaults);
            await _db.SaveChangesAsync();
        }
    }
}