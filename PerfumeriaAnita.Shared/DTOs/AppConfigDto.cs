namespace PerfumeriaAnita.Shared.DTOs;

public class AppConfigDto
{
    public string Clave { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}

public class AparienciaDto
{
    public string NombreTienda { get; set; } = "Perfumería Anita";
    public string Eslogan { get; set; } = "";
    public string ColorPrincipal { get; set; } = "#D4537E";
    public string? LogoBase64 { get; set; }
}
