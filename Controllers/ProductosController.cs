using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeriaAnita.Data;
using PerfumeriaAnita.Models;
using PerfumeriaAnita.Shared.DTOs;

namespace PerfumeriaAnita.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProductosController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<ProductoDto>>> GetAll()
    {
        var list = await _db.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .OrderBy(p => p.Nombre)
            .Select(p => ToDto(p))
            .ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoDto>> GetById(int id)
    {
        var p = await _db.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (p is null) return NotFound();
        return Ok(ToDto(p));
    }

    [HttpGet("buscar/{texto}")]
    public async Task<ActionResult<List<ProductoDto>>> Buscar(string texto)
    {
        var q = texto.ToLower().Trim();
        var list = await _db.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Where(p => p.Activo == true &&
                       (p.Nombre.ToLower().Contains(q) ||
                        (p.CodigoBarras != null && p.CodigoBarras.Contains(q))))
            .Take(8)
            .Select(p => ToDto(p))
            .ToListAsync();
        return Ok(list);
    }

    [HttpGet("barcode/{codigo}")]
    public async Task<ActionResult<ProductoDto>> BuscarPorCodigo(string codigo)
    {
        var p = await _db.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.CodigoBarras == codigo && p.Activo == true && p.Stock > 0);
        if (p is null) return NotFound();
        return Ok(ToDto(p));
    }

    [HttpGet("stock-bajo")]
    public async Task<ActionResult<List<ProductoDto>>> GetStockBajo([FromQuery] int umbral = 5)
    {
        var list = await _db.Productos
            .AsNoTracking()
            .Where(p => p.Activo == true && p.Stock <= umbral)
            .OrderBy(p => p.Stock)
            .Select(p => ToDto(p))
            .ToListAsync();
        return Ok(list);
    }

    [HttpGet("existe-codigo")]
    public async Task<ActionResult<bool>> ExisteCodigo([FromQuery] string codigo, [FromQuery] int excluirId = 0)
    {
        var existe = await _db.Productos.AnyAsync(p => p.CodigoBarras == codigo && p.Id != excluirId);
        return Ok(existe);
    }

    [HttpPost]
    public async Task<ActionResult<ProductoDto>> Create([FromBody] ProductoDto dto)
    {
        var entity = new Producto
        {
            Nombre = dto.Nombre,
            Precio = dto.Precio,
            Stock = dto.Stock,
            CodigoBarras = dto.CodigoBarras,
            CategoriaId = dto.CategoriaId,
            Imagen = dto.Imagen,
            Activo = dto.Activo ?? true
        };
        _db.Productos.Add(entity);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, ToDto(entity));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductoDto dto)
    {
        var entity = await _db.Productos.FindAsync(id);
        if (entity is null) return NotFound();

        entity.Nombre = dto.Nombre;
        entity.Precio = dto.Precio;
        entity.Stock = dto.Stock;
        entity.CodigoBarras = dto.CodigoBarras;
        entity.CategoriaId = dto.CategoriaId;
        if (dto.Imagen is not null) entity.Imagen = dto.Imagen;
        entity.Activo = dto.Activo ?? entity.Activo;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Productos.FindAsync(id);
        if (entity is null) return NotFound();
        entity.Activo = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static ProductoDto ToDto(Producto p) => new()
    {
        Id = p.Id,
        Nombre = p.Nombre,
        Precio = p.Precio,
        Stock = p.Stock,
        CodigoBarras = p.CodigoBarras,
        CategoriaId = p.CategoriaId,
        CategoriaNombre = p.Categoria?.Nombre,
        Imagen = p.Imagen,
        Activo = p.Activo
    };
}
