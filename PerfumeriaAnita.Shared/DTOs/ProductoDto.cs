namespace PerfumeriaAnita.Shared.DTOs;

public class ProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string? CodigoBarras { get; set; }
    public int? CategoriaId { get; set; }
    public string? CategoriaNombre { get; set; }
    public byte[]? Imagen { get; set; }
    public bool? Activo { get; set; } = true;
}
