namespace DirectumCommunity.Services;

public interface IDirectumService
{
    public Task ImportData();
    public Task<bool> Login(string login, string password);
}