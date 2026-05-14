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


    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == request.Correo );


        if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Contrasenia, usuario.Contrasenia))
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
    
      [HttpPost("registro")]
    public async Task<ActionResult<LoginResponseDto>> Registro(Usuario usuario)
    {
        usuario.Contrasenia = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenia);
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
}