using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeriaAnita.Data;
using PerfumeriaAnita.Models;
using PerfumeriaAnita.Shared.DTOs;

namespace PerfumeriaAnita.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VentasController : ControllerBase
{
    private readonly AppDbContext _db;
    public VentasController(AppDbContext db) => _db = db;

    [HttpPost]
    public async Task<ActionResult<ResultadoVentaDto>> RegistrarVenta([FromBody] VentaRequest request)
    {
        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            var venta = new Venta
            {
                Fecha = DateTime.Now,
                MetodoPago = request.MetodoPago,
                Total = 0,
                Detalles = new List<DetalleVenta>()
            };

            decimal total = 0;
            foreach (var item in request.Items)
            {
                var prod = await _db.Productos.FindAsync(item.ProductoId);
                if (prod is null)
                    return BadRequest(new ResultadoVentaDto { Exito = false, Error = $"Producto {item.ProductoId} no encontrado." });
                if (prod.Stock < item.Cantidad)
                    return BadRequest(new ResultadoVentaDto { Exito = false, Error = $"Stock insuficiente para: {prod.Nombre}" });

                prod.Stock -= item.Cantidad;
                var detalle = new DetalleVenta
                {
                    ProductoId = prod.Id,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = prod.Precio
                };
                venta.Detalles.Add(detalle);
                total += detalle.Cantidad * detalle.PrecioUnitario;
            }

            venta.Total = total;
            _db.Ventas.Add(venta);
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new ResultadoVentaDto { Exito = true, Total = total });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, new ResultadoVentaDto { Exito = false, Error = ex.Message });
        }
    }

    [HttpGet("resumen-hoy")]
    public async Task<ActionResult<ResumenDiaDto>> GetResumenHoy()
    {
        var hoy = DateTime.Today;
        var ventasHoy = await _db.Ventas.Where(v => v.Fecha.Date == hoy).ToListAsync();
        return Ok(new ResumenDiaDto
        {
            TotalVendido = ventasHoy.Sum(v => v.Total),
            CantidadVentas = ventasHoy.Count
        });
    }

    [HttpGet("total-ayer")]
    public async Task<ActionResult<decimal>> GetTotalAyer()
    {
        var ayer = DateTime.Today.AddDays(-1);
        var total = await _db.Ventas.Where(v => v.Fecha.Date == ayer).SumAsync(v => v.Total);
        return Ok(total);
    }

    [HttpGet("ultimas")]
    public async Task<ActionResult<List<RegistroVentaDto>>> GetUltimas()
    {
        var ventas = await _db.Ventas
            .AsNoTracking()
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .OrderByDescending(v => v.Fecha)
            .Take(50)
            .ToListAsync();

        var result = ventas.Select(v => new RegistroVentaDto
        {
            IdVenta = $"#{v.Id:D5}",
            NombreProducto = v.Detalles.Count == 1
                ? (v.Detalles.First().Producto?.Nombre ?? "Producto Eliminado")
                : $"{v.Detalles.Count} productos varios",
            Cantidad = v.Detalles.Sum(d => d.Cantidad),
            Precio = v.Total,
            Icono = "receipt_long",
            MetodoPago = v.MetodoPago,
            MetodoPagoCssClass = "badge-green"
        }).ToList();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VentaCompletaDto>> GetById(int id)
    {
        var venta = await _db.Ventas
            .AsNoTracking()
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (venta is null) return NotFound();

        return Ok(new VentaCompletaDto
        {
            Id = venta.Id,
            Fecha = venta.Fecha,
            Total = venta.Total,
            MetodoPago = venta.MetodoPago,
            Detalles = venta.Detalles.Select(d => new DetalleVentaDto
            {
                Id = d.Id,
                VentaId = d.VentaId,
                ProductoId = d.ProductoId,
                ProductoNombre = d.Producto?.Nombre,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario
            }).ToList()
        });
    }
}
