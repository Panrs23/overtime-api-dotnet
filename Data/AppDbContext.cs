using Microsoft.EntityFrameworkCore;
using overtime_api_dotnet.Models;

namespace overtime_api_dotnet.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Empleado> Empleados => Set<Empleado>();
    public DbSet<SolicitudHoraExtra> SolicitudesHorasExtra => Set<SolicitudHoraExtra>();
    public DbSet<EtlLog> EtlLogs => Set<EtlLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.Property(e => e.Nombre).HasMaxLength(120).IsRequired();
            entity.Property(e => e.Correo).HasMaxLength(160).IsRequired();
            entity.Property(e => e.Cargo).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Area).HasMaxLength(100).IsRequired();
            entity.HasIndex(e => e.Correo).IsUnique();
        });

        modelBuilder.Entity<SolicitudHoraExtra>(entity =>
        {
            entity.Property(s => s.Motivo).HasMaxLength(500).IsRequired();
            entity.Property(s => s.Horas).HasPrecision(5, 2);
            entity.Property(s => s.Estado).HasConversion<string>().HasMaxLength(20);
            entity.HasOne(s => s.Empleado)
                .WithMany(e => e.SolicitudesHorasExtra)
                .HasForeignKey(s => s.EmpleadoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<EtlLog>(entity =>
        {
            entity.Property(e => e.NombreArchivo).HasMaxLength(180).IsRequired();
        });
    }
}
