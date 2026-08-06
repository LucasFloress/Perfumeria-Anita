namespace PerfumeriaAnita.Shared.DTOs;

public class ItemVentaRequest
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
}

public class VentaRequest
{
    public List<ItemVentaRequest> Items { get; set; } = new();
    public string MetodoPago { get; set; } = "Efectivo";
}

public class ResultadoVentaDto
{
    public bool Exito { get; set; }
    public decimal Total { get; set; }
    public string? Error { get; set; }
}

public class ResumenDiaDto
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

public class VentaCompletaDto
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public string MetodoPago { get; set; } = "";
    public List<DetalleVentaDto> Detalles { get; set; } = new();
}
