using Hangfire.Server;

namespace DirectumCommunity.Services;

public interface IDirectumService
{
    public Task ImportData(PerformContext context);
    public Task<bool> Login(string login, string password);
}