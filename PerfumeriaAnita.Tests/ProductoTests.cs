using Xunit;
using PerfumeriaAnita.Models;

namespace PerfumeriaAnita.Tests
{
    public class ProductoTests
    {
        [Fact] // Demuestra al IDE que es una prueba que siempre tiene que ser verdad
        public void NuevoProducto_DeberiaTenerStockCeroYEstarActivoPorDefecto()
        {
            // 1. Arrange (Preparar)
            // No necesita preparacion

            // 2. Act (Actuar)
            var productoNuevo = new Producto
            {
                Nombre = "Perfume de Prueba",
                Precio = 15000,
                CodigoBarras = "123456789"
            };

            // 3. Assert (Afirmar)
            // Aqui usamos xUnit para decirle al sistema: "Te exijo que esto sea verdad"
            
            Assert.Equal(0, productoNuevo.Stock);
            Assert.True(productoNuevo.Activo);
        }

        /* EJERCICIO PARA PRACTICAR
        [Fact]
        public void AgregarStock_DeberiaSumarCorrectamenteLasUnidades()
        {
            // 1. Arrange (Preparamos el producto con 10 unidades iniciales)
            var productoNuevo = new Producto
            {
                Nombre = "Jabon Test",
                Precio = 2500,
                Stock = 10,
                CodigoBarras = "987654321"
            };

            // 2. Act (Llamamos a la función AgregarStock pasándole 5)
            productoNuevo.AgregarStock(5);

            // 3. Assert (Le decimos al juez que verifique que el stock ahora sea 15)
            Assert.Equal(15, productoNuevo.Stock);

        }*/

        [Fact]
        public void ReducirStock_DeberiaLanzarExcepcion_CuandoNoHayStockSuficiente()
        {
            // 1 Arrange (Preparamos un producto con poco stock)
            var productoNuevo = new Producto
            {
                Nombre="Perfume Caro",
                Stock=5
            };

            // 2 Y 3. Act y Assert (Se combinan al buscar errores)
            // Le decimos al juez "Asegurate de que esta accion lance un 'InvalidOperationException'"
            var error = Assert.Throws<InvalidOperationException>(() =>
                {
                    // Intentamos sacar 10 unidades cuando solo hay 5
                    productoNuevo.ReducirStock(10);
                }
            );

            // (OPCIONAL extra QA) VAlidamos que el ensaje de error sea exactamente el que pedimos
            Assert.Equal("Stock insuficiente", error.Message);
        }

    }
}