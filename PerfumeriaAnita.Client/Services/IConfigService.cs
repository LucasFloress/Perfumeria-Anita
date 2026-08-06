using PerfumeriaAnita.Shared.DTOs;

namespace PerfumeriaAnita.Client.Services;

public interface IConfigService
{
    Task<Dictionary<string, string>> GetAllAsync();
    Task<string?> GetAsync(string clave);
    Task SetAsync(string clave, string valor, string? descripcion = null);
}
