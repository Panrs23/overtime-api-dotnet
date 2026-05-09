using Microsoft.EntityFrameworkCore;
using overtime_api_dotnet.Data;
using overtime_api_dotnet.Repositories;
using overtime_api_dotnet.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
builder.Services.AddScoped<ISolicitudHoraExtraRepository, SolicitudHoraExtraRepository>();
builder.Services.AddScoped<IEtlLogRepository, EtlLogRepository>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();
builder.Services.AddScoped<ISolicitudHoraExtraService, SolicitudHoraExtraService>();
builder.Services.AddScoped<IReporteService, ReporteService>();
builder.Services.AddScoped<IEtlService, EtlService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
