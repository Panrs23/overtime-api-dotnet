using Microsoft.AspNetCore.Mvc;
using overtime_api_dotnet.DTOs;
using overtime_api_dotnet.Services;

namespace overtime_api_dotnet.Controllers;

[ApiController]
[Route("api/horas-extra")]
public class HorasExtraController(ISolicitudHoraExtraService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<SolicitudHoraExtraDto>>> GetAll() => Ok(await service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SolicitudHoraExtraDto>> GetById(int id)
    {
        var solicitud = await service.GetByIdAsync(id);
        return solicitud is null ? NotFound() : Ok(solicitud);
    }

    [HttpPost]
    public async Task<ActionResult<SolicitudHoraExtraDto>> Create(CrearSolicitudHoraExtraDto dto)
    {
        var creada = await service.CreateAsync(dto);
        if (creada is null)
        {
            return BadRequest("El empleado no existe o no esta activo.");
        }

        return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ActualizarSolicitudHoraExtraDto dto)
    {
        var actualizada = await service.UpdateAsync(id, dto);
        return actualizada ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminada = await service.DeleteAsync(id);
        return eliminada ? NoContent() : NotFound();
    }

    [HttpPut("{id:int}/aprobar")]
    public async Task<IActionResult> Aprobar(int id)
    {
        var actualizada = await service.AprobarAsync(id);
        return actualizada ? NoContent() : NotFound();
    }

    [HttpPut("{id:int}/rechazar")]
    public async Task<IActionResult> Rechazar(int id)
    {
        var actualizada = await service.RechazarAsync(id);
        return actualizada ? NoContent() : NotFound();
    }

    [HttpGet("pendientes")]
    public async Task<ActionResult<List<SolicitudHoraExtraDto>>> GetPendientes() => Ok(await service.GetPendientesAsync());
}
