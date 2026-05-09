using overtime_api_dotnet.DTOs;
using overtime_api_dotnet.Models;
using overtime_api_dotnet.Repositories;

namespace overtime_api_dotnet.Services;

public interface IEmpleadoService
{
    Task<List<EmpleadoDto>> GetAllAsync();
    Task<EmpleadoDto?> GetByIdAsync(int id);
    Task<EmpleadoDto> CreateAsync(CrearEmpleadoDto dto);
    Task<bool> UpdateAsync(int id, ActualizarEmpleadoDto dto);
    Task<bool> DeleteAsync(int id);
}

public class EmpleadoService(IEmpleadoRepository repository) : IEmpleadoService
{
    public async Task<List<EmpleadoDto>> GetAllAsync() =>
        (await repository.GetAllAsync()).Select(ToDto).ToList();

    public async Task<EmpleadoDto?> GetByIdAsync(int id)
    {
        var empleado = await repository.GetByIdAsync(id);
        return empleado is null ? null : ToDto(empleado);
    }

    public async Task<EmpleadoDto> CreateAsync(CrearEmpleadoDto dto)
    {
        var empleado = new Empleado
        {
            Nombre = dto.Nombre,
            Correo = dto.Correo,
            Cargo = dto.Cargo,
            Area = dto.Area,
            Activo = dto.Activo
        };

        await repository.AddAsync(empleado);
        await repository.SaveChangesAsync();
        return ToDto(empleado);
    }

    public async Task<bool> UpdateAsync(int id, ActualizarEmpleadoDto dto)
    {
        var empleado = await repository.GetByIdAsync(id);
        if (empleado is null)
        {
            return false;
        }

        empleado.Nombre = dto.Nombre;
        empleado.Correo = dto.Correo;
        empleado.Cargo = dto.Cargo;
        empleado.Area = dto.Area;
        empleado.Activo = dto.Activo;

        await repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var empleado = await repository.GetByIdAsync(id);
        if (empleado is null)
        {
            return false;
        }

        repository.Delete(empleado);
        await repository.SaveChangesAsync();
        return true;
    }

    private static EmpleadoDto ToDto(Empleado empleado) =>
        new(empleado.Id, empleado.Nombre, empleado.Correo, empleado.Cargo, empleado.Area, empleado.Activo);
}
