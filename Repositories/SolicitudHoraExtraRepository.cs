using Microsoft.EntityFrameworkCore;
using overtime_api_dotnet.Data;
using overtime_api_dotnet.Models;

namespace overtime_api_dotnet.Repositories;

public interface ISolicitudHoraExtraRepository
{
    Task<List<SolicitudHoraExtra>> GetAllAsync();
    Task<SolicitudHoraExtra?> GetByIdAsync(int id);
    Task<List<SolicitudHoraExtra>> GetPendientesAsync();
    Task<List<SolicitudHoraExtra>> GetAprobadasAsync();
    Task AddAsync(SolicitudHoraExtra solicitud);
    Task SaveChangesAsync();
    void Delete(SolicitudHoraExtra solicitud);
}

public class SolicitudHoraExtraRepository(AppDbContext context) : ISolicitudHoraExtraRepository
{
    public Task<List<SolicitudHoraExtra>> GetAllAsync() =>
        context.SolicitudesHorasExtra
            .Include(s => s.Empleado)
            .OrderByDescending(s => s.FechaCreacion)
            .ToListAsync();

    public Task<SolicitudHoraExtra?> GetByIdAsync(int id) =>
        context.SolicitudesHorasExtra
            .Include(s => s.Empleado)
            .FirstOrDefaultAsync(s => s.Id == id);

    public Task<List<SolicitudHoraExtra>> GetPendientesAsync() =>
        context.SolicitudesHorasExtra
            .Include(s => s.Empleado)
            .Where(s => s.Estado == EstadoSolicitud.PENDIENTE)
            .OrderBy(s => s.Fecha)
            .ToListAsync();

    public Task<List<SolicitudHoraExtra>> GetAprobadasAsync() =>
        context.SolicitudesHorasExtra
            .Include(s => s.Empleado)
            .Where(s => s.Estado == EstadoSolicitud.APROBADA)
            .ToListAsync();

    public async Task AddAsync(SolicitudHoraExtra solicitud) =>
        await context.SolicitudesHorasExtra.AddAsync(solicitud);

    public Task SaveChangesAsync() => context.SaveChangesAsync();

    public void Delete(SolicitudHoraExtra solicitud) => context.SolicitudesHorasExtra.Remove(solicitud);
}
