namespace overtime_api_dotnet.DTOs;

public record SolicitudHoraExtraDto(
    int Id,
    int EmpleadoId,
    string EmpleadoNombre,
    DateOnly Fecha,
    decimal Horas,
    string Motivo,
    string Estado,
    DateTime FechaCreacion);

public record CrearSolicitudHoraExtraDto(int EmpleadoId, DateOnly Fecha, decimal Horas, string Motivo);

public record ActualizarSolicitudHoraExtraDto(int EmpleadoId, DateOnly Fecha, decimal Horas, string Motivo);
