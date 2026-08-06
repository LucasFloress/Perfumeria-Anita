using PerfumeriaAnita.Shared.DTOs;

namespace PerfumeriaAnita.Client.Services;

public interface ICategoriaService
{
    Task<List<CategoriaDto>> GetAllAsync();
    Task<CategoriaDto?> CreateAsync(CategoriaDto dto);
    Task<bool> UpdateAsync(int id, CategoriaDto dto);
    Task<(bool Exito, string Mensaje)> DeleteAsync(int id);
}
