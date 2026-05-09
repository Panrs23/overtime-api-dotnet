namespace overtime_api_dotnet.DTOs;

public record EmpleadoDto(int Id, string Nombre, string Correo, string Cargo, string Area, bool Activo);

public record CrearEmpleadoDto(string Nombre, string Correo, string Cargo, string Area, bool Activo);

public record ActualizarEmpleadoDto(string Nombre, string Correo, string Cargo, string Area, bool Activo);
