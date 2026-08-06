using System.Net.Http.Json;
using PerfumeriaAnita.Shared.DTOs;

namespace PerfumeriaAnita.Client.Services;

public class VentasService : IVentaService
{
    private readonly HttpClient _http;
    public VentasService(HttpClient http) => _http = http;

    public async Task<ResultadoVentaDto> RegistrarVentaAsync(VentaRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/ventas", request);
        return (await response.Content.ReadFromJsonAsync<ResultadoVentaDto>())!;
    }

    public async Task<ResumenDiaDto> GetResumenHoyAsync() =>
        await _http.GetFromJsonAsync<ResumenDiaDto>("api/ventas/resumen-hoy") ?? new();

    public async Task<decimal> GetTotalAyerAsync() =>
        await _http.GetFromJsonAsync<decimal>("api/ventas/total-ayer");

    public async Task<List<RegistroVentaDto>> GetUltimasVentasAsync() =>
        await _http.GetFromJsonAsync<List<RegistroVentaDto>>("api/ventas/ultimas") ?? new();

    public async Task<VentaCompletaDto?> GetByIdAsync(int id) =>
        await _http.GetFromJsonAsync<VentaCompletaDto>($"api/ventas/{id}");
}
