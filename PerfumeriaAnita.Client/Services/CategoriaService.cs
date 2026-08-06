using System.Net.Http.Json;
using PerfumeriaAnita.Shared.DTOs;

namespace PerfumeriaAnita.Client.Services;

public class CategoriaService : ICategoriaService
{
    private readonly HttpClient _http;
    public CategoriaService(HttpClient http) => _http = http;

    public async Task<List<CategoriaDto>> GetAllAsync() =>
        await _http.GetFromJsonAsync<List<CategoriaDto>>("api/categorias") ?? new();

    public async Task<CategoriaDto?> CreateAsync(CategoriaDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/categorias", dto);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<CategoriaDto>();
    }

    public async Task<bool> UpdateAsync(int id, CategoriaDto dto)
    {
        var response = await _http.PutAsJsonAsync($"api/categorias/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<(bool Exito, string Mensaje)> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/categorias/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
        {
            var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            return (false, error?.GetValueOrDefault("error") ?? "No se puede eliminar la categoría.");
        }
        if (response.IsSuccessStatusCode)
            return (true, "Categoría eliminada correctamente.");
        return (false, "Error al eliminar la categoría.");
    }
}
