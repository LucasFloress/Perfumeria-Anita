using System.Net.Http.Json;
using PerfumeriaAnita.Shared.DTOs;

namespace PerfumeriaAnita.Client.Services;

public class ConfigService : IConfigService
{
    private readonly HttpClient _http;
    public ConfigService(HttpClient http) => _http = http;

    public async Task<Dictionary<string, string>> GetAllAsync() =>
        await _http.GetFromJsonAsync<Dictionary<string, string>>("api/config") ?? new();

    public async Task<string?> GetAsync(string clave)
    {
        try
        {
            return await _http.GetFromJsonAsync<string>($"api/config/{Uri.EscapeDataString(clave)}");
        }
        catch { return null; }
    }

    public async Task SetAsync(string clave, string valor, string? descripcion = null)
    {
        await _http.PostAsJsonAsync("api/config", new AppConfigDto
        {
            Clave = clave,
            Valor = valor,
            Descripcion = descripcion
        });
    }
}
