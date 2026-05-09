using overtime_api_dotnet.Data;
using overtime_api_dotnet.Models;

namespace overtime_api_dotnet.Repositories;

public interface IEtlLogRepository
{
    Task AddAsync(EtlLog log);
    Task SaveChangesAsync();
}

public class EtlLogRepository(AppDbContext context) : IEtlLogRepository
{
    public async Task AddAsync(EtlLog log) => await context.EtlLogs.AddAsync(log);

    public Task SaveChangesAsync() => context.SaveChangesAsync();
}
