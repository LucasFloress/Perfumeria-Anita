using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeriaAnita.Data;
using PerfumeriaAnita.Models;
using PerfumeriaAnita.Shared.DTOs;

namespace PerfumeriaAnita.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _db;
    public CategoriasController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<CategoriaDto>>> GetAll()
    {
        var list = await _db.Categorias
            .AsNoTracking()
            .Include(c => c.Productos.Where(p => p.Activo == true))
            .Where(c => c.Activo)
            .OrderBy(c => c.Nombre)
            .Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                Activo = c.Activo,
                ProductosCount = c.Productos.Count(p => p.Activo == true)
            })
            .ToListAsync();
        return Ok(list);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> Create([FromBody] CategoriaDto dto)
    {
        var existe = await _db.Categorias.AnyAsync(c => c.Nombre.ToLower() == dto.Nombre.ToLower());
        if (existe)
            return Conflict(new { error = "Ya existe una categoría con ese nombre." });

        var entity = new Categoria
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Activo = true
        };
        _db.Categorias.Add(entity);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new CategoriaDto
        {
            Id = entity.Id,
            Nombre = entity.Nombre,
            Descripcion = entity.Descripcion,
            Activo = entity.Activo
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoriaDto dto)
    {
        var entity = await _db.Categorias.FindAsync(id);
        if (entity is null) return NotFound();

        var duplicado = await _db.Categorias.AnyAsync(c => c.Nombre.ToLower() == dto.Nombre.ToLower() && c.Id != id);
        if (duplicado)
            return Conflict(new { error = "Ya existe una categoría con ese nombre." });

        entity.Nombre = dto.Nombre;
        entity.Descripcion = dto.Descripcion;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Categorias
            .Include(c => c.Productos.Where(p => p.Activo == true))
            .FirstOrDefaultAsync(c => c.Id == id);
        if (entity is null) return NotFound();

        if (entity.Productos.Any(p => p.Activo == true))
            return Conflict(new { error = $"No podés borrar esta categoría porque tiene {entity.Productos.Count} productos activos." });

        entity.Activo = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
