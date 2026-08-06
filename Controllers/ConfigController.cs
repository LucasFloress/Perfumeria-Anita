using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeriaAnita.Data;
using PerfumeriaAnita.Models;
using PerfumeriaAnita.Shared.DTOs;

namespace PerfumeriaAnita.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfigController : ControllerBase
{
    private readonly AppDbContext _db;
    public ConfigController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<Dictionary<string, string>>> GetAll()
    {
        var configs = await _db.AppConfigs.AsNoTracking().ToListAsync();
        var dict = configs
            .GroupBy(c => c.Clave)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(c => c.Id).First().Valor);
        return Ok(dict);
    }

    [HttpGet("{clave}")]
    public async Task<ActionResult<string>> Get(string clave)
    {
        var config = await _db.AppConfigs
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Clave == clave);
        if (config is null) return NotFound();
        return Ok(config.Valor);
    }

    [HttpPost]
    public async Task<IActionResult> Set([FromBody] AppConfigDto dto)
    {
        var existing = await _db.AppConfigs.FirstOrDefaultAsync(c => c.Clave == dto.Clave);
        if (existing is null)
        {
            _db.AppConfigs.Add(new AppConfig
            {
                Clave = dto.Clave,
                Valor = dto.Valor,
                Descripcion = dto.Descripcion,
                UltimaModificacion = DateTime.Now
            });
        }
        else
        {
            existing.Valor = dto.Valor;
            existing.UltimaModificacion = DateTime.Now;
            if (dto.Descripcion is not null) existing.Descripcion = dto.Descripcion;
        }
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("seed")]
    public async Task<IActionResult> Seed()
    {
        if (await _db.AppConfigs.AnyAsync()) return Ok();

        var defaults = new List<AppConfig>
        {
            new() { Clave = AppConfig.Keys.NombreTienda,      Valor = "Perfumería Anita",          Descripcion = "Nombre de la perfumería" },
            new() { Clave = AppConfig.Keys.EsloganTienda,     Valor = "Tu perfume, tu identidad",  Descripcion = "Eslogan visible en la interfaz" },
            new() { Clave = AppConfig.Keys.ColorPrincipal,    Valor = "#D4537E",                   Descripcion = "Color principal (hex)" },
            new() { Clave = AppConfig.Keys.LogoBase64,        Valor = "",                          Descripcion = "Logo en base64" },
            new() { Clave = AppConfig.Keys.ZonaHoraria,       Valor = "America/Argentina/Buenos_Aires", Descripcion = "Zona horaria" },
            new() { Clave = AppConfig.Keys.FormatoHora,       Valor = "24",                        Descripcion = "Formato de hora" },
            new() { Clave = AppConfig.Keys.EmailNotif,        Valor = "anita@perfumeria.com",      Descripcion = "Email de notificaciones" },
            new() { Clave = AppConfig.Keys.TelefonoNotif,     Valor = "",                          Descripcion = "Teléfono para alertas" },
            new() { Clave = AppConfig.Keys.NotifNuevoPedido,  Valor = "true",                      Descripcion = "Notificar nuevo pedido" },
            new() { Clave = AppConfig.Keys.NotifStockBajo,    Valor = "true",                      Descripcion = "Notificar stock bajo" },
            new() { Clave = AppConfig.Keys.NotifTurno,        Valor = "true",                      Descripcion = "Recordatorio de turno" },
            new() { Clave = AppConfig.Keys.NotifResena,       Valor = "false",                     Descripcion = "Notificar reseña" },
            new() { Clave = AppConfig.Keys.NotifInforme,      Valor = "true",                      Descripcion = "Informe semanal" },
            new() { Clave = AppConfig.Keys.NotifPush,         Valor = "true",                      Descripcion = "Notificaciones push" },
            new() { Clave = AppConfig.Keys.StockMinimoAlerta, Valor = "5",                         Descripcion = "Stock mínimo para alerta" },
            new() { Clave = AppConfig.Keys.PorcentajeAumento, Valor = "30",                        Descripcion = "Porcentaje de aumento" },
            new() { Clave = AppConfig.Keys.MetodoPagoDefault, Valor = "Efectivo",                  Descripcion = "Método de pago por defecto" },
        };

        _db.AppConfigs.AddRange(defaults);
        await _db.SaveChangesAsync();
        return Ok();
    }
}
