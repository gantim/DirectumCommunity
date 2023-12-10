using System.Text;
using DirectumCommunity.Models;
using Microsoft.EntityFrameworkCore;
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

    public async Task ImportData()
    {
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
            .FindEntriesAsync();

        using (var db = new ApplicationDbContext())
    {
        foreach (var item in result)
        {
            item.DepartmentId = item.Department != null ? item.Department.Id : null;
            item.PersonId = item.Person != null ? item.Person.Id : null;
            item.JobTitleId = item.JobTitle != null ? item.JobTitle.Id : null;

            if (item.Department != null)
        {
            var existingDepartment = await db.Departments.FindAsync(item.Department.Id);

            if (existingDepartment != null)
            {
                if (!db.ChangeTracker.Entries().Any(e => e.Entity == existingDepartment))
                {
                    db.Entry(existingDepartment).CurrentValues.SetValues(item.Department);
                }
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
                {
                    db.Entry(existingJobTitle).CurrentValues.SetValues(item.JobTitle);
                }
            }
            else
            {
                db.JobTitles.Add(item.JobTitle);
            }
        }

        if (item.Person != null)
        {
            var existingPerson = await db.Persons.FirstOrDefaultAsync(p => p.Id == item.Person.Id);

            if (existingPerson != null)
            {
                if (!db.ChangeTracker.Entries().Any(e => e.Entity == existingPerson))
                {
                    db.Entry(existingPerson).CurrentValues.SetValues(item.Person);
                }
            }
            else
            {
                db.Persons.Add(item.Person);
            }
        }

        item.Department = null;
        item.JobTitle = null;
        item.Person = null;

        item.LastModifyDate = DateTime.Now;

        var existingEmployee = await db.Employees.FindAsync(item.Id);

            if (existingEmployee != null)
            {
                db.Entry(existingEmployee).CurrentValues.SetValues(item);
            }
            else
            {
                db.Employees.Add(item);
            }
        }

        await db.SaveChangesAsync();
    }
        
    }
}