using Microsoft.AspNetCore.Mvc;
using overtime_api_dotnet.DTOs;
using overtime_api_dotnet.Services;

namespace overtime_api_dotnet.Controllers;

[ApiController]
[Route("api/reportes")]
public class ReportesController(IReporteService service) : ControllerBase
{
    [HttpGet("horas-aprobadas")]
    public async Task<ActionResult<List<HorasAprobadasDto>>> GetHorasAprobadas() =>
        Ok(await service.GetHorasAprobadasAsync());
}
