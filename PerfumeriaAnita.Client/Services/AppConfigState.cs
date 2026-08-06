namespace PerfumeriaAnita.Client.Services;

public class AppConfigState
{
    public string NombreTienda { get; private set; } = "Perfumería Anita";
    public string Eslogan { get; private set; } = "Tu perfume, tu identidad";
    public string ColorPrincipal { get; private set; } = "#D4537E";
    public string? LogoBase64 { get; private set; } = null;

    public event Action? OnChange;

    public void Inicializar(Dictionary<string, string> configs)
    {
        NombreTienda = configs.GetValueOrDefault("tienda.nombre", "Perfumería Anita");
        Eslogan = configs.GetValueOrDefault("tienda.eslogan", "Tu perfume, tu identidad");
        ColorPrincipal = configs.GetValueOrDefault("tienda.color_principal", "#D4537E");
        var logo = configs.GetValueOrDefault("tienda.logo_base64", "");
        LogoBase64 = string.IsNullOrEmpty(logo) ? null : logo;
    }

    public void ActualizarApariencia(string nombre, string eslogan, string color, string? logoBase64)
    {
        NombreTienda = nombre;
        Eslogan = eslogan;
        ColorPrincipal = color;
        LogoBase64 = logoBase64;
        NotificarCambio();
    }

    public void ActualizarColor(string color)
    {
        ColorPrincipal = color;
        NotificarCambio();
    }

    private void NotificarCambio() => OnChange?.Invoke();
}
