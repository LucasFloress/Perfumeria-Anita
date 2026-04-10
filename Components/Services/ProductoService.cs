using Microsoft.EntityFrameworkCore;
using PerfumeriaAnita.Data;
using PerfumeriaAnita.Models;

namespace PerfumeriaAnita.Services;

public class ProductoService : IProductoService
{
    private readonly AppDbContext _context;

    public ProductoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Producto>> BuscarProductosAsync(string texto)
    {
        var q = texto.ToLower().Trim();
        return await _context.Productos
            .Where(p => p.Activo == true && 
                       (p.Nombre.ToLower().Contains(q) || 
                       (p.CodigoBarras != null && p.CodigoBarras.Contains(q))))
            .Take(8) // Máximo 8 resultados para no saturar el dropdown
            .ToListAsync();
    }

    public async Task<List<Producto>> GetProductosBajoStockAsync(int umbral = 5)
    {
        return await _context.Productos
            .Where(p => p.Activo == true && p.Stock <= umbral)
            .OrderBy(p => p.Stock)
            .ToListAsync();
    }
}