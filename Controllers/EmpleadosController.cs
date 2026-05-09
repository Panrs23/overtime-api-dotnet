using Microsoft.AspNetCore.Mvc;
using overtime_api_dotnet.DTOs;
using overtime_api_dotnet.Services;

namespace overtime_api_dotnet.Controllers;

[ApiController]
[Route("api/empleados")]
public class EmpleadosController(IEmpleadoService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<EmpleadoDto>>> GetAll() => Ok(await service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmpleadoDto>> GetById(int id)
    {
        var empleado = await service.GetByIdAsync(id);
        return empleado is null ? NotFound() : Ok(empleado);
    }

    [HttpPost]
    public async Task<ActionResult<EmpleadoDto>> Create(CrearEmpleadoDto dto)
    {
        var creado = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ActualizarEmpleadoDto dto)
    {
        var actualizado = await service.UpdateAsync(id, dto);
        return actualizado ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await service.DeleteAsync(id);
        return eliminado ? NoContent() : NotFound();
    }
}
