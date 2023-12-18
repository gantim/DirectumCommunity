using Hangfire.Server;

namespace DirectumCommunity.Services;

public interface IDirectumService
{
    public Task ImportEmployees(PerformContext context);
    public Task ImportSubstitutions(PerformContext context);
    public Task<bool> Login(string login, string password);
}