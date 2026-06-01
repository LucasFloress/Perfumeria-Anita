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

    public async Task<Producto?> BuscarPorCodigoAsync(string CodigoBarras)
    {
        return await _context.Productos
            .FirstOrDefaultAsync(p => p.CodigoBarras == CodigoBarras && p.Activo == true && p.Stock > 0);
    }

    public async Task<Producto?> GetProductoByIdAsync(int id)
    {
        return await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> ExisteCodigoBarrasAsync(string codigo, int idAExcluir)
    {
        return await _context.Productos
            .AnyAsync(p => p.CodigoBarras == codigo && p.Id != idAExcluir);
    }

    public async Task<bool> ActualizarProductoAsync(Producto producto)
    {
        try
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> EliminarProductoAsync(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null) return false;
        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<List<Categoria>> GetCategoriasActivasAsync()
    {
        return await _context.Categorias
            .Where(c => c.Activo == true)
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }

    public async Task<bool> GuardarProductoAsync(Producto producto)
    {
        try
        {
            if (producto.Id == 0)
            {
                _context.Productos.Add(producto);
            }
            else
            {
                // Update marca todas las propiedades como modificadas
                _context.Productos.Update(producto);
            }

            var resultado = await _context.SaveChangesAsync();
            return resultado > 0; // Retorna true si realmente se guardó algo
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar en SQLite: {ex.Message}");
            return false;
        }
    }

    public async Task<List<Producto>> GetProductosAsync()
    {
        return await _context.Productos
            .AsNoTracking() // <--- Esto obliga a leer siempre de la base de datos real
            .Include(p => p.Categoria)
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

}
