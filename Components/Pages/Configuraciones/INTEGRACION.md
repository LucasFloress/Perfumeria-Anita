# Integración de la página de Configuraciones
## PerfumeriaAnita — Blazor Server

---

### 1. Copiar los archivos al proyecto

```
Models/       → AppConfig.cs
Services/     → ConfigService.cs
Pages/        → Configuraciones.razor
Pages/        → Configuraciones.razor.css
Migrations/   → AddAppConfigs.cs   (o usá dotnet ef directamente)
```

---

### 2. Agregar DbSet al ApplicationDbContext

```csharp
// En ApplicationDbContext.cs
public DbSet<AppConfig> AppConfigs { get; set; }
```

---

### 3. Registrar el servicio en Program.cs

```csharp
// En Program.cs, junto a los demás servicios
builder.Services.AddScoped<IConfigService, ConfigService>();
```

---

### 4. Aplicar la migración

```bash
# Opción A: usar la migración incluida
dotnet ef database update

# Opción B: crear una migración nueva (si ya tenés migraciones existentes)
dotnet ef migrations add AddAppConfigs
dotnet ef database update
```

---

### 5. Agregar el link al menú de navegación

```razor
<!-- En NavMenu.razor -->
<NavLink href="configuraciones" Match="NavLinkMatch.All">
    <span class="bi bi-gear-fill" aria-hidden="true"></span> Configuraciones
</NavLink>
```

---

### 6. (Opcional) Leer el color desde cualquier componente

Si querés aplicar el color guardado en otros componentes:

```razor
@inject IConfigService ConfigService

@code {
    private string _color = "#D4537E";

    protected override async Task OnInitializedAsync()
    {
        _color = await ConfigService.GetAsync(AppConfig.Keys.ColorPrincipal, "#D4537E");
    }
}
```

---

### Claves disponibles (AppConfig.Keys)

| Constante            | Clave en BD                        | Valor default                          |
|----------------------|------------------------------------|----------------------------------------|
| NombreTienda         | tienda.nombre                      | Anita                                  |
| EsloganTienda        | tienda.eslogan                     | Tu perfume, tu identidad               |
| ColorPrincipal       | tienda.color_principal             | #D4537E                              |
| ZonaHoraria          | sistema.zona_horaria               | America/Argentina/Buenos_Aires         |
| FormatoHora          | sistema.formato_hora               | 24                                     |
| EmailNotif           | notif.email                        | anita@perfumeria.com                   |
| TelefonoNotif        | notif.telefono                     | (vacío)                                |
| NotifNuevoPedido     | notif.nuevo_pedido                 | true                                   |
| NotifStockBajo       | notif.stock_bajo                   | true                                   |
| NotifTurno           | notif.recordatorio_turno           | true                                   |
| NotifResena          | notif.resena                       | false                                  |
| NotifInforme         | notif.informe_semanal              | true                                   |
| NotifPush            | notif.push_panel                   | true                                   |
| StockMinimoAlerta    | inventario.stock_minimo            | 5                                      |
| PorcentajeAumento    | _porcentajeAumento                 | 30                                     |
