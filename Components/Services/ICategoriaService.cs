using PerfumeriaAnita.Models;

namespace PerfumeriaAnita.Services;

public interface ICategoriaService
{
    // Trae todas las categorías activas, incluyendo cuántos productos tienen
    Task<List<Categoria>> GetCategoriasActivasAsync();
    
    // Guarda una nueva o edita una existente
    Task<bool> GuardarCategoriaAsync(Categoria categoria);
    
    // Intenta dar de baja una categoría (Falla si tiene productos activos)
    Task<(bool Exito, string Mensaje)> EliminarCategoriaAsync(int id);
}