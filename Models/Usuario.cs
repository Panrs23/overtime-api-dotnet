namespace overtime_api_dotnet.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Contrasenia { get; set; } = string.Empty;
    public string Rol { get; set; } = "Empleado";

}