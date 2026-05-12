using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using overtime_api_dotnet.Data;
using overtime_api_dotnet.DTOs;
using overtime_api_dotnet.Models;


namespace overtime_api_dotnet.Controllers; 

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

      [HttpPost("registro")]
    public async Task<ActionResult<LoginResponseDto>> Registro(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return new LoginResponseDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Correo = usuario.Correo,
            Contrasenia = usuario.Contrasenia,
            Rol = usuario.Rol
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == request.Correo && u.Contrasenia == request.Contrasenia);

        if (usuario == null)
        {
            return Unauthorized(new { message = "Correo o contraseña incorrectos" });
        }

        var response = new LoginResponseDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Correo = usuario.Correo,
            Contrasenia = usuario.Contrasenia,
            Rol = usuario.Rol
        };

        return Ok(response);
    }
}