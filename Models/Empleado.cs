namespace overtime_api_dotnet.Models;

public class Empleado
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    public List<SolicitudHoraExtra> SolicitudesHorasExtra { get; set; } = [];
}
