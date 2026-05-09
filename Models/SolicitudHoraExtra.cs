namespace overtime_api_dotnet.Models;

public class SolicitudHoraExtra
{
    public int Id { get; set; }
    public int EmpleadoId { get; set; }
    public DateOnly Fecha { get; set; }
    public decimal Horas { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public EstadoSolicitud Estado { get; set; } = EstadoSolicitud.PENDIENTE;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public Empleado? Empleado { get; set; }
}
