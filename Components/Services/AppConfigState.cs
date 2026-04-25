using PerfumeriaAnita.Services;
using PerfumeriaAnita.Models;

namespace PerfumeriaAnita.Services
{
    /// <summary>
    /// Singleton que mantiene en memoria las configuraciones visuales activas.
    /// Al cambiar cualquier valor, notifica a todos los componentes suscritos
    /// (NavMenu, layouts, etc.) para que se actualicen sin recargar la página.
    /// </summary>
    public class AppConfigState
    {
        // ── Valores en memoria ─────────────────────────────────────────────
        public string NombreTienda   { get; private set; } = "Perfumería Anita";
        public string Eslogan        { get; private set; } = "Tu perfume, tu identidad";
        public string ColorPrincipal { get; private set; } = "#D4537E";

        // Logo: guardado en BD como base64 (data:image/...;base64,...)
        public string? LogoBase64    { get; private set; } = null;

        // ── Evento que dispara re-render en los suscriptores ───────────────
        public event Action? OnChange;

        // ── Inicialización desde BD (llamar una vez al arrancar) ───────────
        public async Task InicializarAsync(IConfigService configService)
        {
            var all = await configService.GetAllAsync();

            NombreTienda   = all.GetValueOrDefault(AppConfig.Keys.NombreTienda,   "Perfumería Anita");
            Eslogan        = all.GetValueOrDefault(AppConfig.Keys.EsloganTienda,  "Tu perfume, tu identidad");
            ColorPrincipal = all.GetValueOrDefault(AppConfig.Keys.ColorPrincipal, "#D4537E");
            LogoBase64     = all.GetValueOrDefault(AppConfig.Keys.LogoBase64,     null!);

            // No notificamos acá porque todavía no hay suscriptores
        }

        // ── Métodos de actualización (llamar desde Configuraciones.razor) ──
        public void ActualizarApariencia(string nombre, string eslogan, string color, string? logoBase64)
        {
            NombreTienda   = nombre;
            Eslogan        = eslogan;
            ColorPrincipal = color;
            LogoBase64     = logoBase64;
            NotificarCambio();
        }

        public void ActualizarColor(string color)
        {
            ColorPrincipal = color;
            NotificarCambio();
        }

        private void NotificarCambio() => OnChange?.Invoke();
    }
}
