using PerfumeriaAnita.Models;

namespace PerfumeriaAnita.Data
{
    public static class ProductoSeeder
    {
        public static List<Producto> GetProductos()
        {
            return new List<Producto>
            {
                // ─── LIMPIEZA ───────────────────────────────────────────

                new Producto
                {
                    Id = 1,
                    Nombre = "Lavandina Concentrada Ayudín",
                    Precio = 1250.00m,
                    Stock = 80,
                    CodigoBarras = "7790580123401",
                    CategoriaId = 1,
                    Activo = true
                },
                new Producto
                {
                    Id = 2,
                    Nombre = "Detergente Magistral Limón 750ml",
                    Precio = 980.00m,
                    Stock = 120,
                    CodigoBarras = "7790580234502",
                    CategoriaId = 1,
                    Activo = true
                },
                new Producto
                {
                    Id = 3,
                    Nombre = "Limpiador Multiuso Mr. Músculo Cocina",
                    Precio = 1540.00m,
                    Stock = 55,
                    CodigoBarras = "7790580345603",
                    CategoriaId = 1,
                    Activo = true
                },
                new Producto
                {
                    Id = 4,
                    Nombre = "Desengrasante Blem 500ml",
                    Precio = 870.00m,
                    Stock = 40,
                    CodigoBarras = "7790580456704",
                    CategoriaId = 1,
                    Activo = true
                },
                new Producto
                {
                    Id = 5,
                    Nombre = "Suavizante Comfort Lavanda 2L",
                    Precio = 2100.00m,
                    Stock = 65,
                    CodigoBarras = "7790580567805",
                    CategoriaId = 1,
                    Activo = true
                },

                // ─── PERFUMES ───────────────────────────────────────────

                new Producto
                {
                    Id = 6,
                    Nombre = "Perfume Mujer Luna Rossa 60ml",
                    Precio = 8500.00m,
                    Stock = 30,
                    CodigoBarras = "7791234567001",
                    CategoriaId = 2,
                    Activo = true
                },
                new Producto
                {
                    Id = 7,
                    Nombre = "Perfume Hombre Black Code 100ml",
                    Precio = 9200.00m,
                    Stock = 25,
                    CodigoBarras = "7791234567002",
                    CategoriaId = 2,
                    Activo = true
                },
                new Producto
                {
                    Id = 8,
                    Nombre = "Colonia Unisex Fresh Breeze 80ml",
                    Precio = 4300.00m,
                    Stock = 50,
                    CodigoBarras = "7791234567003",
                    CategoriaId = 2,
                    Activo = true
                },
                new Producto
                {
                    Id = 9,
                    Nombre = "Perfume Niña Candy Dream 50ml",
                    Precio = 3800.00m,
                    Stock = 20,
                    CodigoBarras = "7791234567004",
                    CategoriaId = 2,
                    Activo = true
                },
                new Producto
                {
                    Id = 10,
                    Nombre = "Set Regalo Perfume + Body Lotion Rose",
                    Precio = 12500.00m,
                    Stock = 15,
                    CodigoBarras = "7791234567005",
                    CategoriaId = 2,
                    Activo = true
                },
            };
        }

        public static List<Categoria> GetCategorias()
        {
            return new List<Categoria>
            {
                new Categoria { Id = 1, Nombre = "Limpieza", Descripcion = "Productos de limpieza del hogar", Activo = true },
                new Categoria { Id = 2, Nombre = "Perfumes", Descripcion = "Perfumes y colonias", Activo = true },
            };
        }
    }
}