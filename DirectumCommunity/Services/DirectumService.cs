using System.Text;
using DirectumCommunity.Models;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using Simple.OData.Client;

namespace DirectumCommunity.Services;

public class DirectumService : IDirectumService
{
    private readonly string _host;
    private readonly string _login;
    private readonly string _password;
    protected PerformContext _context;

    public DirectumService(string host, string login, string password)
    {
        if (string.IsNullOrEmpty(host)) throw new ArgumentException(host);
        if (string.IsNullOrEmpty(login)) throw new ArgumentException(login);
        if (string.IsNullOrEmpty(password)) throw new ArgumentException(password);

        _host = host;
        _login = login;
        _password = password;
    }

    public async Task ImportData(PerformContext context)
    {
        _context = context;
        
        _context.WriteLine("Начало импорта данных сотрудников из DirectumRx...");
        
        var odataClientSettings = new ODataClientSettings(new Uri(_host));

        odataClientSettings.BeforeRequest += message =>
        {
            var authenticationHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_login}:{_password}"));
            message.Headers.Add("Authorization", "Basic " + authenticationHeaderValue);
        };
        var odataClient = new ODataClient(odataClientSettings);

        var result = await odataClient.For<Employee>("IEmployees")
            .Expand(x => x.Department)
            .Expand(x => x.JobTitle)
            .Expand(x => x.Person)
            .Expand(x => x.Login)
            .Expand(x => x.PersonalPhoto)
            .FindEntriesAsync();

        var bar = context.WriteProgressBar();
        
        using (var db = new ApplicationDbContext())
        {
            var employeePhotos = await db.PersonalPhotos.ToListAsync();
            
            foreach (var item in result.ToList().WithProgress(bar))
            {
                try
                {
                    _context.WriteLine($"Импорт сотрудника: {item}");

                    item.DepartmentId = item.Department != null ? item.Department.Id : null;
                    item.PersonId = item.Person != null ? item.Person.Id : null;
                    item.JobTitleId = item.JobTitle != null ? item.JobTitle.Id : null;
                    item.LoginId = item.Login != null ? item.Login.Id : null;

                    if (item.Department != null)
                    {
                        var existingDepartment = await db.Departments.FindAsync(item.Department.Id);

                        if (existingDepartment != null)
                        {
                            if (!db.ChangeTracker.Entries().Any(e => e.Entity == existingDepartment))
                                db.Entry(existingDepartment).CurrentValues.SetValues(item.Department);
                        }
                        else
                        {
                            db.Departments.Add(item.Department);
                        }
                    }

                    if (item.JobTitle != null)
                    {
                        var existingJobTitle = await db.JobTitles.FindAsync(item.JobTitle.Id);

                        if (existingJobTitle != null)
                        {
                            if (!db.ChangeTracker.Entries().Any(e => e.Entity == existingJobTitle))
                                db.Entry(existingJobTitle).CurrentValues.SetValues(item.JobTitle);
                        }
                        else
                        {
                            db.JobTitles.Add(item.JobTitle);
                        }
                    }

                    if (item.Person != null)
                    {
                        var existingPerson = await db.Persons.FindAsync(item.Person.Id);

                        if (existingPerson != null)
                        {
                            if (!db.ChangeTracker.Entries().Any(e => e.Entity == existingPerson))
                                db.Entry(existingPerson).CurrentValues.SetValues(item.Person);
                        }
                        else
                        {
                            db.Persons.Add(item.Person);
                        }
                    }

                    if (item.Login != null)
                    {
                        var existingLogin = await db.Logins.FindAsync(item.Login.Id);

                        if (existingLogin != null)
                        {
                            if (!db.ChangeTracker.Entries().Any(e => e.Entity == existingLogin))
                                db.Entry(existingLogin).CurrentValues.SetValues(item.Login);
                        }
                        else
                        {
                            db.Logins.Add(item.Login);
                        }
                    }

                    item.Department = null;
                    item.JobTitle = null;
                    item.Person = null;
                    item.Login = null;

                    item.LastModifyDate = DateTime.Now;

                    var existingEmployee = await db.Employees.FindAsync(item.Id);

                    if (existingEmployee != null)
                    {
                        if (item.PersonalPhoto != null)
                        {
                            db.PersonalPhotos.RemoveRange(employeePhotos.Where(ep => ep.PersonalPhotoHash == item.PersonalPhotoHash));
                            item.PersonalPhoto.PersonalPhotoHash = item.PersonalPhotoHash;
                            db.PersonalPhotos.Add(item.PersonalPhoto);
                        }

                        db.Entry(existingEmployee).CurrentValues.SetValues(item);
                    }
                    else
                    {
                        db.Employees.Add(item);
                    }

                }
                catch (Exception e)
                {
                    _context.WriteLine($"Ошибка импорта {item}: {e.Message} ");
                }
            }

            await db.SaveChangesAsync();
        }
    }

    public async Task<bool> Login(string login, string password)
    {
        var requestUrl = $"{_host}$metadata/";
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        var authenticationHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{login}:{password}"));
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authenticationHeaderValue);

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}