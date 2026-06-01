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

    /// <summary>
    /// Busca un producto por su código de barras.
    /// </summary>
    Task<Models.Producto?> BuscarPorCodigoAsync(string codigo);

    /// <summary>
    /// Elimina un producto.
    /// </summary>
    Task<bool> EliminarProductoAsync(int id);

    /// <summary>
    /// Crea un nuevo producto.
    /// </summary>
    Task<bool> ExisteCodigoBarrasAsync(string codigoBarras, int idAExcluir);

    /// <summary>
    /// Obtiene un producto por su ID.
    /// </summary>
    Task<Models.Producto?> GetProductoByIdAsync(int id);

}
