using Microsoft.AspNetCore.Mvc;
using overtime_api_dotnet.DTOs;
using overtime_api_dotnet.Services;

namespace overtime_api_dotnet.Controllers;

[ApiController]
[Route("api/etl")]
public class EtlController(IEtlService service) : ControllerBase
{
    [HttpPost("cargar-empleados")]
    public async Task<ActionResult<EtlResultadoDto>> CargarEmpleados(CargarEmpleadosCsvDto dto)
    {
        try
        {
            return Ok(await service.CargarEmpleadosAsync(dto.RutaArchivo));
        }
        catch (FileNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
