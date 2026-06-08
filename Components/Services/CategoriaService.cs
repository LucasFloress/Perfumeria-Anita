using Microsoft.EntityFrameworkCore;
using PerfumeriaAnita.Data;
using PerfumeriaAnita.Models;

namespace PerfumeriaAnita.Services;

public class CategoriaService : ICategoriaService
{
    private readonly AppDbContext _context;

    public CategoriaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Categoria>> GetCategoriasActivasAsync()
    {
        // Traemos las categorías activas e INCLUIMOS sus productos para poder contarlos en la tabla
        return await _context.Categorias
            .Include(c => c.Productos.Where(p => p.Activo == true)) 
            .Where(c => c.Activo)
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }

    public async Task<bool> GuardarCategoriaAsync(Categoria categoria)
    {
        var nombreExiste = await _context.Categorias
            .AnyAsync(c => c.Nombre.ToLower() == categoria.Nombre.ToLower() && c.Id != categoria.Id);

        if (nombreExiste)
        {
            // Tiramos un error personalizado que el modal va a atrapar y mostrar en rojo
            throw new Exception("Ya existe una categoría con ese nombre. Por favor, elegí otro.");
        }

        // Si el nombre está libre, procedemos a guardar normal
        if (categoria.Id == 0)
        {
            var nuevaCategoria = new Categoria 
            {
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Activo = true
            };
            _context.Categorias.Add(nuevaCategoria);
        }
        else
        {
            var existente = await _context.Categorias.FindAsync(categoria.Id);
            if (existente != null)
            {
                existente.Nombre = categoria.Nombre;
                existente.Descripcion = categoria.Descripcion;
            }
        }
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(bool Exito, string Mensaje)> EliminarCategoriaAsync(int id)
    {
        var categoria = await _context.Categorias
            .Include(c => c.Productos.Where(p => p.Activo == true))
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria == null)
            return (false, "Categoría no encontrada.");

        // REGLA DE NEGOCIO: No se puede borrar si tiene productos activos
        if (categoria.Productos.Any())
        {
            return (false, $"No podés borrar esta categoría porque tiene {categoria.Productos.Count} productos asociados. Primero cambiales la categoría o borralos.");
        }

        try
        {
            // Baja lógica
            categoria.Activo = false;
            await _context.SaveChangesAsync();
            return (true, "Categoría eliminada correctamente.");
        }
        catch (Exception ex)
        {
            return (false, $"Error al eliminar: {ex.Message}");
        }
    }
}