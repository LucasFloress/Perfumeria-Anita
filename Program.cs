using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using PerfumeriaAnita.Components;
using PerfumeriaAnita.Data;
using PerfumeriaAnita.Services;

var builder = WebApplication.CreateBuilder(args);

/* --- 1. Configurar la BD --- */
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

/* --- 2. Configurar MudBlazor --- */
builder.Services.AddMudServices();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IVentaService, VentaService>();

builder.Services.AddScoped<IConfigService, ConfigService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

builder.Services.AddSingleton<AppConfigState>(); 


var app = builder.Build();

/* --- 3. Pipelines HTTP --- */
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await DataSeeder.SeedAsync(db);
}

using (var scope = app.Services.CreateScope())
{
    var state   = app.Services.GetRequiredService<AppConfigState>();
    var service = scope.ServiceProvider.GetRequiredService<IConfigService>();
    await state.InicializarAsync(service);
}

app.Run();