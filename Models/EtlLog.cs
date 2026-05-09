namespace overtime_api_dotnet.Models;

public class EtlLog
{
    public int Id { get; set; }
    public string NombreArchivo { get; set; } = string.Empty;
    public int RegistrosProcesados { get; set; }
    public DateTime FechaEjecucion { get; set; } = DateTime.UtcNow;
}
