using overtime_api_dotnet.DTOs;
using overtime_api_dotnet.Models;
using overtime_api_dotnet.Repositories;

namespace overtime_api_dotnet.Services;

public interface IEtlService
{
    Task<EtlResultadoDto> CargarEmpleadosAsync(string rutaArchivo);
}

public class EtlService(IEmpleadoRepository empleadoRepository, IEtlLogRepository etlLogRepository) : IEtlService
{
    public async Task<EtlResultadoDto> CargarEmpleadosAsync(string rutaArchivo)
    {
        if (!File.Exists(rutaArchivo))
        {
            throw new FileNotFoundException("No se encontro el archivo CSV.", rutaArchivo);
        }

        var lineas = await File.ReadAllLinesAsync(rutaArchivo);
        var procesados = 0;

        // CSV simple: se asume que los datos no contienen comas dentro de los campos.
        foreach (var linea in lineas.Skip(1).Where(l => !string.IsNullOrWhiteSpace(l)))
        {
            var columnas = linea.Split(',', StringSplitOptions.TrimEntries);
            if (columnas.Length < 5)
            {
                continue;
            }

            var correo = columnas[1];
            var existente = await empleadoRepository.GetByCorreoAsync(correo);
            if (existente is not null)
            {
                continue;
            }

            var empleado = new Empleado
            {
                Nombre = columnas[0],
                Correo = correo,
                Cargo = columnas[2],
                Area = columnas[3],
                Activo = bool.TryParse(columnas[4], out var activo) && activo
            };

            await empleadoRepository.AddAsync(empleado);
            procesados++;
        }

        var fecha = DateTime.UtcNow;
        await etlLogRepository.AddAsync(new EtlLog
        {
            NombreArchivo = Path.GetFileName(rutaArchivo),
            RegistrosProcesados = procesados,
            FechaEjecucion = fecha
        });

        await empleadoRepository.SaveChangesAsync();
        await etlLogRepository.SaveChangesAsync();

        return new EtlResultadoDto(Path.GetFileName(rutaArchivo), procesados, fecha);
    }
}
