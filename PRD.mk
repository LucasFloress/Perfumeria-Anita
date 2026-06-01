📄 Product Requirements Document (PRD) - Perfumería Anita

1. Resumen del Proyecto

Nombre: Perfumería Anita - Admin Dashboard
Descripción: Un sistema de gestión (Backoffice/Dashboard) web para administrar el inventario y las ventas de una perfumería local. El enfoque principal es la rapidez de uso en el punto de venta, incluyendo alertas de stock, métricas visuales y un módulo preparado para escáner de código de barras.

2. Stack Tecnológico Actual

 El proyecto ya está inicializado y estructurado bajo las siguientes tecnologías:

    Framework Principal: .NET 9 con Blazor (Web App / Interactive Server).

    Base de Datos: SQLite (archivo local .db ignorado en Git).

    ORM: Entity Framework Core (EF Core) con migraciones activas.

    Estilos: * Estructura base (Layout/Sidebar) con CSS puro clásico.

        Vistas modernas (Dashboard de Ventas) integradas con Tailwind CSS (vía CDN).

    Entorno de Desarrollo: Docker / DevContainers preconfigurado para entorno Linux.

3. Estado Actual y Funcionalidades Desarrolladas

    El sistema ya cuenta con el cascarón principal y dos vistas clave operativas (UI + Lógica de presentación):

    A. Layout Principal (MainLayout.razor)

        Estructura de navegación con Sidebar izquierdo fijo (NavMenu) y área de contenido central fluida usando Flexbox.

    B. Módulo de Inventario (Index.razor en /Productos)

        Listado: Tabla de productos con imagen genérica, SKU, Categoría, Precio y Stock.

        Paginación: Sistema de paginación funcional (5 ítems por página).

        Filtros: Búsqueda en tiempo real por nombre o código, filtrado por categoría y estado.

        Escáner: Lógica de UI para recibir input de un lector de códigos de barra (busca strings de +8 dígitos numéricos).

        Alertas: Tarjetas de advertencia automática para productos con stock crítico (≤ 5 unidades) o agotados.

        Estado visual: Cálculos automáticos de estado ("EN STOCK", "STOCK BAJO", "AGOTADO", "INACTIVO") representados con badges de colores.

    C. Módulo de Ventas (Ventas.razor)

        Dashboard Visual: Tarjetas de métricas (Ventas de hoy).

        Gráficos: Representación visual (SVG/CSS) de la distribución de ventas por método de pago.

        Tabla de Transacciones: Últimos productos vendidos con detalles de precio, cantidad y método de pago (con clases de colores de Tailwind).

        Resumen Financiero: Cálculo de Total Bruto, Impuestos y Neto Estimado.

    D. Modelos de Datos (Entities principales)

        Producto: Id, Nombre, Precio (decimal), Stock, CodigoBarras, CategoriaId, Imagen (byte[]), Activo, DetallesVenta.

        Categoria: Id, Nombre.

        (Nota: Existen modelos auxiliares de UI para el Dashboard que eventualmente deben conectarse a datos reales).

4. Requerimientos Próximos (Lo que necesito desarrollar) 

        [x] Tarea 1: Crear los formularios CRUD (Crear, Editar, Eliminar) reales para el modelo Producto conectando con EF Core.

        [x] Tarea 2: Crear la vista de "Punto de Venta (POS)" para agregar productos a un carrito y generar una venta real descontando stock.

        [x] Tarea 3: Unificar el diseño para que todo el proyecto migre completamente a Tailwind CSS o se estandarice en CSS puro.

5. ⚠️ Reglas Estrictas para el LLM (Developer Guidelines)   

    Como desarrollador Junior a cargo de este proyecto, necesito que respetes estas reglas al generar código:

        Cero React/JavaScript Frameworks: Todo el comportamiento dinámico debe resolverse con C# y Blazor (@code, @bind, @onclick). No generes hooks (useState) ni código JSX.

        Respeta EF Core: Usa inyección de dependencias (@inject AppDbContext Contexto) para las consultas. Prefiere LINQ y métodos asíncronos (ToListAsync()).

        No sobreescribas el Layout: El MainLayout.razor ya maneja el menú lateral. Las páginas nuevas solo deben preocuparse por su contenido interno.

        Código Modular: Si una vista (como el inventario o el POS) se vuelve muy grande, sugiere dividirla en componentes pequeños (ej. <TablaProductos />, <FiltrosBusqueda />).

        Explicaciones claras: Explica brevemente el "por qué" de la lógica en C#, especialmente si usas funciones avanzadas de .NET 9.
