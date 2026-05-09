using overtime_api_dotnet.DTOs;
using overtime_api_dotnet.Repositories;

namespace overtime_api_dotnet.Services;

public interface IReporteService
{
    Task<List<HorasAprobadasDto>> GetHorasAprobadasAsync();
}

public class ReporteService(ISolicitudHoraExtraRepository repository) : IReporteService
{
    public async Task<List<HorasAprobadasDto>> GetHorasAprobadasAsync() =>
        (await repository.GetAprobadasAsync())
            .GroupBy(s => new { s.EmpleadoId, Nombre = s.Empleado?.Nombre ?? string.Empty })
            .Select(g => new HorasAprobadasDto(g.Key.EmpleadoId, g.Key.Nombre, g.Sum(s => s.Horas)))
            .OrderBy(r => r.EmpleadoNombre)
            .ToList();
}
