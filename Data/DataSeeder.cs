using Microsoft.EntityFrameworkCore;
using PerfumeriaAnita.Data;
using PerfumeriaAnita.Models;

namespace PerfumeriaAnita.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        // ── 1. Categorías ──────────────────────────────────────────────
        if (!await db.Categorias.AnyAsync())
        {
            var categorias = new List<Categoria>
            {
                new() { Nombre = "Perfumes Mujer" },
                new() { Nombre = "Perfumes Hombre" },
                new() { Nombre = "Perfumes Unisex" },
                new() { Nombre = "Colonias" },
                new() { Nombre = "Sets y Regalos" },
                new() { Nombre = "Desodorantes" },
                new() { Nombre = "Cremas y Lociones" },
                new() { Nombre = "Accesorios" },
            };
            await db.Categorias.AddRangeAsync(categorias);
            await db.SaveChangesAsync();
        }

        // ── 2. Productos ───────────────────────────────────────────────
        if (!await db.Productos.AnyAsync())
        {
            // Traemos las categorías ya guardadas para referenciarlas por nombre
            var cats = await db.Categorias.ToDictionaryAsync(c => c.Nombre);

            var productos = new List<Producto>
            {
                // Perfumes Mujer
                new() { Nombre = "Perfume Mujer Luna Rossa 60ml",         Precio = 8500.00m,  Stock = 30,  CodigoBarras = "7791234560001", CategoriaId = cats["Perfumes Mujer"].Id,    Activo = true },
                new() { Nombre = "Perfume Mujer Flower by Kenzo 50ml",    Precio = 11200.00m, Stock = 20,  CodigoBarras = "7791234560002", CategoriaId = cats["Perfumes Mujer"].Id,    Activo = true },
                new() { Nombre = "Perfume Mujer 212 Carolina Herrera",    Precio = 13500.00m, Stock = 15,  CodigoBarras = "7791234560003", CategoriaId = cats["Perfumes Mujer"].Id,    Activo = true },

                // Perfumes Hombre
                new() { Nombre = "Perfume Hombre Black Code 100ml",       Precio = 9200.00m,  Stock = 25,  CodigoBarras = "7791234561001", CategoriaId = cats["Perfumes Hombre"].Id,   Activo = true },
                new() { Nombre = "Perfume Hombre 1 Million Paco R.",      Precio = 14800.00m, Stock = 18,  CodigoBarras = "7791234561002", CategoriaId = cats["Perfumes Hombre"].Id,   Activo = true },
                new() { Nombre = "Perfume Hombre Acqua di Gio 75ml",      Precio = 12300.00m, Stock = 22,  CodigoBarras = "7791234561003", CategoriaId = cats["Perfumes Hombre"].Id,   Activo = true },

                // Perfumes Unisex
                new() { Nombre = "Perfume Unisex CK One 100ml",           Precio = 9800.00m,  Stock = 35,  CodigoBarras = "7791234562001", CategoriaId = cats["Perfumes Unisex"].Id,   Activo = true },
                new() { Nombre = "Perfume Unisex Fresh Breeze 80ml",      Precio = 7600.00m,  Stock = 40,  CodigoBarras = "7791234562002", CategoriaId = cats["Perfumes Unisex"].Id,   Activo = true },

                // Colonias
                new() { Nombre = "Colonia Infantil Johnson Baby 200ml",   Precio = 3200.00m,  Stock = 50,  CodigoBarras = "7791234563001", CategoriaId = cats["Colonias"].Id,          Activo = true },
                new() { Nombre = "Colonia Clásica Lavanda 250ml",         Precio = 2800.00m,  Stock = 45,  CodigoBarras = "7791234563002", CategoriaId = cats["Colonias"].Id,          Activo = true },

                // Sets y Regalos
                new() { Nombre = "Set Regalo Perfume + Body Lotion",      Precio = 12500.00m, Stock = 15,  CodigoBarras = "7791234564001", CategoriaId = cats["Sets y Regalos"].Id,    Activo = true },
                new() { Nombre = "Set Duo Hombre Black + Desodorante",    Precio = 10900.00m, Stock = 12,  CodigoBarras = "7791234564002", CategoriaId = cats["Sets y Regalos"].Id,    Activo = true },

                // Desodorantes
                new() { Nombre = "Desodorante Rexona Women 150ml",        Precio = 2100.00m,  Stock = 80,  CodigoBarras = "7791234565001", CategoriaId = cats["Desodorantes"].Id,      Activo = true },
                new() { Nombre = "Desodorante Axe Dark Temptation",       Precio = 2300.00m,  Stock = 75,  CodigoBarras = "7791234565002", CategoriaId = cats["Desodorantes"].Id,      Activo = true },

                // Cremas y Lociones
                new() { Nombre = "Crema Corporal Nivea Hidratante 400ml", Precio = 3800.00m,  Stock = 60,  CodigoBarras = "7791234566001", CategoriaId = cats["Cremas y Lociones"].Id, Activo = true },
                new() { Nombre = "Loción Corporal Avon Soft & Sensual",   Precio = 2950.00m,  Stock = 55,  CodigoBarras = "7791234566002", CategoriaId = cats["Cremas y Lociones"].Id, Activo = true },

                // Accesorios
                new() { Nombre = "Atomizador de Bolsillo 10ml",           Precio = 1500.00m,  Stock = 100, CodigoBarras = "7791234567001", CategoriaId = cats["Accesorios"].Id,        Activo = true },
                new() { Nombre = "Bolsa de Regalo Premium",               Precio = 850.00m,   Stock = 200, CodigoBarras = "7791234567002", CategoriaId = cats["Accesorios"].Id,        Activo = true },
            };

            await db.Productos.AddRangeAsync(productos);
            await db.SaveChangesAsync();
        }
    }
}