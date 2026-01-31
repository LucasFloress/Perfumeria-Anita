using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using PerfumeriaAnita.Components;

var builder = WebApplication.CreateBuilder(args);

/* --- 1. Configurar la BD --- */
builder.Services.AddDbContext<PerfumeriaAnita.Data.AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

/* --- 2. Configurar MudBlazor --- */
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// --- 3. Configurar los pipelines de los HTTPS Request ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
