using Microsoft.EntityFrameworkCore;
using PerfumeriaAnita.Data;
using PerfumeriaAnita.Models;

namespace PerfumeriaAnita.Services;

public class VentaService : IVentaService
{
    private readonly AppDbContext _context;

    public VentaService(AppDbContext context)
    {
        _context = context;
    }

    // 1. Método para Ventas Rápidas (Un solo producto)
    public async Task<ResultadoVenta> RegistrarVentaRapidaAsync(int productoId, int cantidad)
    {
        var producto = await _context.Productos.FindAsync(productoId);
        
        if (producto == null) return new ResultadoVenta { Exito = false, Error = "Producto no encontrado." };
        if (producto.Stock < cantidad) return new ResultadoVenta { Exito = false, Error = "Stock insuficiente." };

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Descontamos stock
            producto.Stock -= cantidad;
            decimal totalVenta = producto.Precio * cantidad;

            // Creamos la Venta (Cabecera) y su Detalle (Renglón) al mismo tiempo
            var nuevaVenta = new Venta
            {
                Fecha = DateTime.Now,
                MetodoPago = "Efectivo",
                Total = totalVenta,
                Detalles = new List<DetalleVenta>
                {
                    new DetalleVenta
                    {
                        ProductoId = producto.Id,
                        Cantidad = cantidad,
                        PrecioUnitario = producto.Precio
                    }
                }
            };

            _context.Ventas.Add(nuevaVenta);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new ResultadoVenta { Exito = true, Total = totalVenta };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new ResultadoVenta { Exito = false, Error = ex.Message };
        }
    }

    // 2. Método para Ventas con Carrito (Varios productos de golpe)
    public async Task<ResultadoVenta> RegistrarVentaCarritoAsync(List<ItemVentaDto> items, string metodoPago = "Efectivo")
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            var nuevaVenta = new Venta
            {
                Fecha = DateTime.Now,
                MetodoPago = metodoPago,
                Total = 0, 
                Detalles = new List<DetalleVenta>()
            };

            decimal totalCalculado = 0;

            foreach (var item in items)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                
                if (producto == null) throw new Exception("Un producto del carrito ya no existe.");
                if (producto.Stock < item.Cantidad) throw new Exception($"Stock insuficiente para: {producto.Nombre}");

                producto.Stock -= item.Cantidad;

                var detalle = new DetalleVenta
                {
                    ProductoId = producto.Id,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = producto.Precio
                };

                nuevaVenta.Detalles.Add(detalle);
                totalCalculado += (detalle.Cantidad * detalle.PrecioUnitario);
            }

            nuevaVenta.Total = totalCalculado;
            _context.Ventas.Add(nuevaVenta);
            
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new ResultadoVenta { Exito = true, Total = totalCalculado };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new ResultadoVenta { Exito = false, Error = ex.Message };
        }
    }

    // 3. Método para obtener el Resumen del Día
    public async Task<ResumenDia> GetResumenHoyAsync()
    {
        var hoy = DateTime.Today;
        
        var ventasDeHoy = await _context.Ventas
            .Where(v => v.Fecha.Date == hoy)
            .ToListAsync();

        return new ResumenDia
        {
            TotalVendido = ventasDeHoy.Sum(v => v.Total),
            CantidadVentas = ventasDeHoy.Count
        };
    }

    // 4. Método para alimentar la tabla de la página de Ventas
    public async Task<List<RegistroVentaDto>> GetUltimasVentasAsync()
    {
        // Traemos las ventas INCLUYENDO sus detalles y los datos de los productos
        var ventasDB = await _context.Ventas
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .OrderByDescending(v => v.Fecha)
            .Take(50)
            .ToListAsync();

        var historial = ventasDB.Select(v => new RegistroVentaDto
        {
            IdVenta = $"#{v.Id:D5}", // Le pone ceros adelante (Ej: #00014)
            // Lógica inteligente: Si hay 1 detalle, muestra el nombre. Si hay varios, dice "X productos varios".
            NombreProducto = v.Detalles.Count == 1 
                             ? (v.Detalles.First().Producto?.Nombre ?? "Producto Eliminado") 
                             : $"{v.Detalles.Count} productos varios",
            Cantidad = v.Detalles.Sum(d => d.Cantidad),
            Precio = v.Total,
            Icono = "receipt_long",
            MetodoPago = v.MetodoPago,
            MetodoPagoCssClass = "badge-green"
        }).ToList();

        return historial;
    }
}