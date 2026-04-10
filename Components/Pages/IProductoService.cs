namespace PerfumeriaAnita.Services;

public interface IProductoService
{
    /// <summary>
    /// Busca productos cuyo nombre o código contenga el texto dado.
    /// Devuelve máximo 8 resultados ordenados por relevancia.
    /// </summary>
    Task<List<Models.Producto>> BuscarProductosAsync(string texto);

    /// <summary>
    /// Devuelve todos los productos con stock menor o igual al umbral indicado.
    /// </summary>
    Task<List<Models.Producto>> GetProductosBajoStockAsync(int umbral = 5);
}
