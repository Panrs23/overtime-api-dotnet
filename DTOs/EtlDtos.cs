namespace overtime_api_dotnet.DTOs;

public record CargarEmpleadosCsvDto(string RutaArchivo);

public record EtlResultadoDto(string NombreArchivo, int RegistrosProcesados, DateTime FechaEjecucion);
