using Microsoft.EntityFrameworkCore;
using overtime_api_dotnet.Data;
using overtime_api_dotnet.Models;

namespace overtime_api_dotnet.Repositories;

public interface IEmpleadoRepository
{
    Task<List<Empleado>> GetAllAsync();
    Task<Empleado?> GetByIdAsync(int id);
    Task<Empleado?> GetByCorreoAsync(string correo);
    Task AddAsync(Empleado empleado);
    Task SaveChangesAsync();
    void Delete(Empleado empleado);
}

public class EmpleadoRepository(AppDbContext context) : IEmpleadoRepository
{
    public Task<List<Empleado>> GetAllAsync() =>
        context.Empleados.OrderBy(e => e.Nombre).ToListAsync();

    public Task<Empleado?> GetByIdAsync(int id) =>
        context.Empleados.FirstOrDefaultAsync(e => e.Id == id);

    public Task<Empleado?> GetByCorreoAsync(string correo) =>
        context.Empleados.FirstOrDefaultAsync(e => e.Correo == correo);

    public async Task AddAsync(Empleado empleado) =>
        await context.Empleados.AddAsync(empleado);

    public Task SaveChangesAsync() => context.SaveChangesAsync();

    public void Delete(Empleado empleado) => context.Empleados.Remove(empleado);
}
