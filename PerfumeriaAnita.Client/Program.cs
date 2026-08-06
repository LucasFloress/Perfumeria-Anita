using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using PerfumeriaAnita.Client.Services;
using PerfumeriaAnita.Shared.DTOs;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<PerfumeriaAnita.Client.Components.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IVentaService, VentasService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IConfigService, ConfigService>();
builder.Services.AddSingleton<AppConfigState>();

await builder.Build().RunAsync();
