namespace PerfumeriaAnita.Services;

public interface IVentaService
{
    // Mantenemos el de venta rápida por si lo usas en otro lado
    Task<ResultadoVenta> RegistrarVentaRapidaAsync(int productoId, int cantidad);
    
    // NUEVO: Método para procesar el carrito completo de una sola vez
    Task<ResultadoVenta> RegistrarVentaCarritoAsync(List<ItemVentaDto> items, string metodoPago = "Efectivo");

    Task<ResumenDia> GetResumenHoyAsync();
    Task<List<RegistroVentaDto>> GetUltimasVentasAsync();
}

// DTO para transportar los items del carrito al servicio
public class ItemVentaDto
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
}

public class ResultadoVenta
{
    public bool Exito { get; set; }
    public decimal Total { get; set; }
    public string? Error { get; set; }
}

public class ResumenDia
{
    public decimal TotalVendido { get; set; }
    public int CantidadVentas { get; set; }
}

public class RegistroVentaDto
{
    public string IdVenta { get; set; } = string.Empty;
    public string NombreProducto { get; set; } = string.Empty;
    public string Icono { get; set; } = "shopping_bag";
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
    public string MetodoPago { get; set; } = "Efectivo";
    public string MetodoPagoCssClass { get; set; } = "badge-green";
}