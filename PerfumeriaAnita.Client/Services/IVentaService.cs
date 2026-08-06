using PerfumeriaAnita.Shared.DTOs;

namespace PerfumeriaAnita.Client.Services;

public interface IVentaService
{
    Task<ResultadoVentaDto> RegistrarVentaAsync(VentaRequest request);
    Task<ResumenDiaDto> GetResumenHoyAsync();
    Task<decimal> GetTotalAyerAsync();
    Task<List<RegistroVentaDto>> GetUltimasVentasAsync();
    Task<VentaCompletaDto?> GetByIdAsync(int id);
}
