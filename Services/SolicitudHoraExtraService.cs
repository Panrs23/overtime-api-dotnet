using overtime_api_dotnet.DTOs;
using overtime_api_dotnet.Models;
using overtime_api_dotnet.Repositories;

namespace overtime_api_dotnet.Services;

public interface ISolicitudHoraExtraService
{
    Task<List<SolicitudHoraExtraDto>> GetAllAsync();
    Task<SolicitudHoraExtraDto?> GetByIdAsync(int id);
    Task<List<SolicitudHoraExtraDto>> GetPendientesAsync();
    Task<SolicitudHoraExtraDto?> CreateAsync(CrearSolicitudHoraExtraDto dto);
    Task<bool> UpdateAsync(int id, ActualizarSolicitudHoraExtraDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> AprobarAsync(int id);
    Task<bool> RechazarAsync(int id);
}

public class SolicitudHoraExtraService(
    ISolicitudHoraExtraRepository solicitudRepository,
    IEmpleadoRepository empleadoRepository) : ISolicitudHoraExtraService
{
    public async Task<List<SolicitudHoraExtraDto>> GetAllAsync() =>
        (await solicitudRepository.GetAllAsync()).Select(ToDto).ToList();

    public async Task<SolicitudHoraExtraDto?> GetByIdAsync(int id)
    {
        var solicitud = await solicitudRepository.GetByIdAsync(id);
        return solicitud is null ? null : ToDto(solicitud);
    }

    public async Task<List<SolicitudHoraExtraDto>> GetPendientesAsync() =>
        (await solicitudRepository.GetPendientesAsync()).Select(ToDto).ToList();

    public async Task<SolicitudHoraExtraDto?> CreateAsync(CrearSolicitudHoraExtraDto dto)
    {
        var empleado = await empleadoRepository.GetByIdAsync(dto.EmpleadoId);
        if (empleado is null || !empleado.Activo)
        {
            return null;
        }

        var solicitud = new SolicitudHoraExtra
        {
            EmpleadoId = dto.EmpleadoId,
            Fecha = dto.Fecha,
            Horas = dto.Horas,
            Motivo = dto.Motivo,
            Estado = EstadoSolicitud.PENDIENTE,
            FechaCreacion = DateTime.UtcNow
        };

        await solicitudRepository.AddAsync(solicitud);
        await solicitudRepository.SaveChangesAsync();

        solicitud.Empleado = empleado;
        return ToDto(solicitud);
    }

    public async Task<bool> UpdateAsync(int id, ActualizarSolicitudHoraExtraDto dto)
    {
        var solicitud = await solicitudRepository.GetByIdAsync(id);
        var empleado = await empleadoRepository.GetByIdAsync(dto.EmpleadoId);
        if (solicitud is null || empleado is null || !empleado.Activo)
        {
            return false;
        }

        solicitud.EmpleadoId = dto.EmpleadoId;
        solicitud.Fecha = dto.Fecha;
        solicitud.Horas = dto.Horas;
        solicitud.Motivo = dto.Motivo;

        await solicitudRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var solicitud = await solicitudRepository.GetByIdAsync(id);
        if (solicitud is null)
        {
            return false;
        }

        solicitudRepository.Delete(solicitud);
        await solicitudRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AprobarAsync(int id) => await CambiarEstadoAsync(id, EstadoSolicitud.APROBADA);

    public async Task<bool> RechazarAsync(int id) => await CambiarEstadoAsync(id, EstadoSolicitud.RECHAZADA);

    private async Task<bool> CambiarEstadoAsync(int id, EstadoSolicitud estado)
    {
        var solicitud = await solicitudRepository.GetByIdAsync(id);
        if (solicitud is null)
        {
            return false;
        }

        solicitud.Estado = estado;
        await solicitudRepository.SaveChangesAsync();
        return true;
    }

    private static SolicitudHoraExtraDto ToDto(SolicitudHoraExtra solicitud) =>
        new(
            solicitud.Id,
            solicitud.EmpleadoId,
            solicitud.Empleado?.Nombre ?? string.Empty,
            solicitud.Fecha,
            solicitud.Horas,
            solicitud.Motivo,
            solicitud.Estado.ToString(),
            solicitud.FechaCreacion);
}
