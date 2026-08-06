using System.Net.Http.Json;
using PerfumeriaAnita.Shared.DTOs;

namespace PerfumeriaAnita.Client.Services;

public class ProductoService : IProductoService
{
    private readonly HttpClient _http;
    public ProductoService(HttpClient http) => _http = http;

    public async Task<List<ProductoDto>> GetAllAsync() =>
        await _http.GetFromJsonAsync<List<ProductoDto>>("api/productos") ?? new();

    public async Task<ProductoDto?> GetByIdAsync(int id) =>
        await _http.GetFromJsonAsync<ProductoDto>($"api/productos/{id}");

    public async Task<List<ProductoDto>> BuscarAsync(string texto) =>
        await _http.GetFromJsonAsync<List<ProductoDto>>($"api/productos/buscar/{Uri.EscapeDataString(texto)}") ?? new();

    public async Task<ProductoDto?> BuscarPorCodigoAsync(string codigo) =>
        await _http.GetFromJsonAsync<ProductoDto>($"api/productos/barcode/{Uri.EscapeDataString(codigo)}");

    public async Task<List<ProductoDto>> GetStockBajoAsync(int umbral = 5) =>
        await _http.GetFromJsonAsync<List<ProductoDto>>($"api/productos/stock-bajo?umbral={umbral}") ?? new();

    public async Task<bool> ExisteCodigoAsync(string codigo, int excluirId = 0) =>
        await _http.GetFromJsonAsync<bool>($"api/productos/existe-codigo?codigo={Uri.EscapeDataString(codigo)}&excluirId={excluirId}");

    public async Task<ProductoDto?> CreateAsync(ProductoDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/productos", dto);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<ProductoDto>();
    }

    public async Task<bool> UpdateAsync(int id, ProductoDto dto)
    {
        var response = await _http.PutAsJsonAsync($"api/productos/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/productos/{id}");
        return response.IsSuccessStatusCode;
    }
}
