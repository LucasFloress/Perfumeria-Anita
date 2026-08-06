using PerfumeriaAnita.Shared.DTOs;

namespace PerfumeriaAnita.Client.Services;

public interface IProductoService
{
    Task<List<ProductoDto>> GetAllAsync();
    Task<ProductoDto?> GetByIdAsync(int id);
    Task<List<ProductoDto>> BuscarAsync(string texto);
    Task<ProductoDto?> BuscarPorCodigoAsync(string codigo);
    Task<List<ProductoDto>> GetStockBajoAsync(int umbral = 5);
    Task<bool> ExisteCodigoAsync(string codigo, int excluirId = 0);
    Task<ProductoDto?> CreateAsync(ProductoDto dto);
    Task<bool> UpdateAsync(int id, ProductoDto dto);
    Task<bool> DeleteAsync(int id);
}
