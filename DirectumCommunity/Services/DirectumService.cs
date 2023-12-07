using System.Text;
using Simple.OData.Client;

namespace DirectumCommunity.Services;

public class DirectumService : IDirectumService
{
    private readonly string _host;
    private readonly string _login;
    private readonly string _password;

    public DirectumService(string host, string login, string password)
    {
        if(string.IsNullOrEmpty(host)) throw new ArgumentException(host);
        if(string.IsNullOrEmpty(login)) throw new ArgumentException(login);
        if(string.IsNullOrEmpty(password)) throw new ArgumentException(password);
        
        _host = host;
        _login = login;
        _password = password;
    }

    public async Task GetAllPersons()
    {
        var odataClientSettings = new ODataClientSettings(new Uri(_host));

        odataClientSettings.BeforeRequest += (HttpRequestMessage message) =>
        {
            var authenticationHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_login}:{_password}"));
            message.Headers.Add("Authorization", "Basic " + authenticationHeaderValue);
        };
        var odataClient = new ODataClient(odataClientSettings);
        
        var associatedApplications = await odataClient.FindEntriesAsync("IPersons");
        foreach (var associatedApplication in associatedApplications)
        foreach (var property in associatedApplication)
            Console.WriteLine($"{property.Key}: {property.Value}");
    }
}