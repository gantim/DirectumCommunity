using System.Net.Http.Headers;
using System.Text;
using DirectumCommunity.Models;
using DirectumCommunity.Models.Responses;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

    [JobDisplayName("Импорт данных о сотрудниках")]
    public async Task ImportEmployees(PerformContext context)
    {
        _context = context;

        await ImportOrganization();
        
        _context.WriteLine("Начало импорта данных сотрудников из DirectumRx...");

        try
        {
            using (var client = new HttpClient())
            {
                var authenticationHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_login}:{_password}"));
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", authenticationHeaderValue);

                HttpResponseMessage response = await client.GetAsync(
                    $"{_host}IEmployees?$expand=Department, JobTitle, Login, PersonalPhoto, Person($expand=City)");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<EmployeeResponse>(json);

                    if (result?.Value != null)
                    {
                        await using (var db = new ApplicationDbContext())
                        {
                            var progressBar = context.WriteProgressBar();

                            var employeePhotos = await db.PersonalPhotos.ToListAsync();
                            foreach (var employee in result.Value.WithProgress(progressBar))
                            {
                                try
                                {
                                    _context.WriteLine($"Импорт сотрудника: {employee}");

                                    employee.DepartmentId = employee.Department?.Id;
                                    employee.PersonId = employee.Person?.Id;
                                    employee.JobTitleId = employee.JobTitle?.Id;
                                    employee.LoginId = employee.Login?.Id;

                                    if (employee.Person != null)
                                    {
                                        employee.Person.CityId = employee.Person?.City?.Id;
                                    }

                                    if (employee.Department != null)
                                    {
                                        var existingDepartment = await db.Departments.FindAsync(employee.Department.Id);

                                        if (employee.Department.Id != existingDepartment?.Id)
                                        {
                                            await SetChanges(employee.Department, existingDepartment,
                                                employee.PersonId.Value, db);
                                        }
                                        
                                        if (existingDepartment != null)
                                        {
                                            existingDepartment.Update(employee.Department);
                                        }
                                        else
                                        {
                                            db.Departments.Add(employee.Department);
                                        }
                                    }

                                    if (employee.JobTitle != null)
                                    {
                                        var existingJobTitle = await db.JobTitles.FindAsync(employee.JobTitle.Id);

                                        if (employee.JobTitle.Id != existingJobTitle?.Id)
                                        {
                                            await SetChanges(employee.JobTitle, existingJobTitle,
                                                employee.PersonId.Value, db);
                                        }
                                        
                                        if (existingJobTitle != null)
                                        {
                                            existingJobTitle.Update(employee.JobTitle);
                                        }
                                        else
                                        {
                                            db.JobTitles.Add(employee.JobTitle);
                                        }
                                    }

                                    if (employee.Person.City != null)
                                    {
                                        var existingCity = await db.Cities.FindAsync(employee.Person.City.Id);

                                        if (existingCity != null)
                                        {
                                            existingCity.Update(employee.Person.City);
                                        }
                                        else
                                        {
                                            db.Cities.Add(employee.Person.City);
                                        }
                                    }

                                    if (employee.Person != null)
                                    {
                                        var existingPerson = await db.Persons.FindAsync(employee.Person.Id);

                                        if (employee.Person.LastName != existingPerson?.LastName)
                                        {
                                            await SetChanges(employee.Person, existingPerson,
                                                employee.PersonId.Value, db);
                                        }
                                        
                                        if (existingPerson != null)
                                        {
                                            existingPerson.Update(employee.Person);
                                        }
                                        else
                                        {
                                            db.Persons.Add(employee.Person);
                                        }
                                    }

                                    if (employee.Login != null)
                                    {
                                        var existingLogin = await db.Logins.FindAsync(employee.Login.Id);

                                        if (existingLogin != null)
                                        {
                                            existingLogin.Update(employee.Login);
                                        }
                                        else
                                        {
                                            db.Logins.Add(employee.Login);
                                        }
                                    }

                                    employee.Department = null;
                                    employee.JobTitle = null;
                                    if (employee.Person != null) employee.Person.City = null;
                                    employee.Person = null;
                                    employee.Login = null;

                                    employee.LastModifyDate = DateTime.Now;

                                    var existingEmployee = await db.Employees.FindAsync(employee.Id);

                                    if (existingEmployee != null)
                                    {
                                        if (employee.PersonalPhoto != null)
                                        {
                                            db.PersonalPhotos.RemoveRange(employeePhotos.Where(ep =>
                                                ep.PersonalPhotoHash == employee.PersonalPhotoHash));
                                            employee.PersonalPhoto.PersonalPhotoHash = employee.PersonalPhotoHash;
                                            db.PersonalPhotos.Add(employee.PersonalPhoto);
                                        }

                                        existingEmployee.Update(employee);
                                    }
                                    else
                                    {
                                        employee.CreateDate = DateTimeOffset.Now.ToOffset(TimeSpan.Zero);
                                        db.Employees.Add(employee);
                                    }
                                }
                                catch (Exception e)
                                {
                                    _context.WriteLine($"Ошибка импорта {employee}: {e.Message} ", ConsoleTextColor.Red);
                                }
                            }

                            await db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        _context.WriteLine($"Данные для импорта отсутствуют");
                    }
                }
                else
                {
                    _context.WriteLine($"Ошибка при выполнении запроса: {response.StatusCode}", ConsoleTextColor.Red);
                }
            }
        }
        catch (Exception e)
        {
            _context.WriteLine($"Ошибка: {e.InnerException?.Message}", ConsoleTextColor.Red);
        }
        finally
        {
            _context.WriteLine($"Импорт сотрудников завершен...", ConsoleTextColor.Green);
        }
    }

    [JobDisplayName("Импорт данных о замещении")]
    public async Task ImportSubstitutions(PerformContext context)
    {
        _context = context;
        _context.WriteLine("Начало импорта данных о замещении из DirectumRx...");

        try
        {
            using (var client = new HttpClient())
            {
                var authenticationHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_login}:{_password}"));
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", authenticationHeaderValue);

                HttpResponseMessage response = await client.GetAsync(
                    $"{_host}ISubstitutions?$expand=*");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<SubstitutionsResponse>(json);

                    if (result?.Value != null)
                    {
                        await using (var db = new ApplicationDbContext())
                        {
                            var progressBar = _context.WriteProgressBar();
                            
                            foreach (var substitution in result.Value.WithProgress(progressBar))
                            {
                                try
                                {
                                    _context.WriteLine($"Импорт замещения: {substitution.Name}");
                                    
                                    substitution.UserId = substitution.User?.Id;
                                    substitution.SubstituteId = substitution.Substitute?.Id;

                                    substitution.User = null;
                                    substitution.Substitute = null;
                                    
                                    var existingSubstitution = await db.Substitutions.FindAsync(substitution.Id);

                                    if (existingSubstitution != null)
                                    {
                                        existingSubstitution.Update(substitution);
                                    }
                                    else
                                    {
                                        db.Substitutions.Add(substitution);
                                    }
                                }
                                catch (Exception e)
                                {
                                    _context.WriteLine($"Ошибка импорта {substitution.Name}: {e.Message}", ConsoleTextColor.Red);
                                }
                            }

                            await db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        _context.WriteLine($"Данные для импорта отсутствуют", ConsoleTextColor.Red);
                    }
                }
                else
                {
                    _context.WriteLine($"Ошибка при выполнении запроса: {response.StatusCode}", ConsoleTextColor.Red);
                }
            }
        }
        catch (Exception e)
        {
            _context.WriteLine($"Ошибка импорта замещений: {e.InnerException?.Message}", ConsoleTextColor.Red);
        }
        finally
        {
            _context.WriteLine($"Импорт замещений завершен...", ConsoleTextColor.Green);
        }
    }
    
    private async Task ImportOrganization()
    {
        _context.WriteLine("Начало импорта данных организации из DirectumRx...");

        try
        {
            using (var client = new HttpClient())
            {
                var authenticationHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_login}:{_password}"));
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", authenticationHeaderValue);

                HttpResponseMessage response = await client.GetAsync(
                    $"{_host}IBusinessUnits?$filter=Status eq 'Active'");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<OrganizationResponse>(json);

                    if (result?.Value != null)
                    {
                        await using (var db = new ApplicationDbContext())
                        {
                            var progressBar = _context.WriteProgressBar();
                            
                            foreach (var organization in result.Value.WithProgress(progressBar))
                            {
                                try
                                {
                                    _context.WriteLine($"Импорт организации: {organization.Name}");

                                    var existingOrganization = await db.Organizations.FindAsync(organization.Id);

                                    if (existingOrganization != null)
                                    {
                                        existingOrganization.Update(organization);
                                    }
                                    else
                                    {
                                        db.Organizations.Add(organization);
                                    }
                                }
                                catch (Exception e)
                                {
                                    _context.WriteLine($"Ошибка импорта {organization.Name}: {e.Message}", ConsoleTextColor.Red);
                                }
                            }

                            await db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        _context.WriteLine($"Данные для импорта отсутствуют", ConsoleTextColor.Red);
                    }
                }
                else
                {
                    _context.WriteLine($"Ошибка при выполнении запроса: {response.StatusCode}", ConsoleTextColor.Red);
                }
            }
        }
        catch (Exception e)
        {
            _context.WriteLine($"Ошибка импорта организации: {e.InnerException?.Message}", ConsoleTextColor.Red);
        }
        finally
        {
            _context.WriteLine($"Импорт организаций завершен...", ConsoleTextColor.Green);
        }
    }
    
    public async Task<bool> Login(string login, string password)
    {
        var requestUrl = $"{_host}$metadata/";
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        var authenticationHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{login}:{password}"));
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Basic", authenticationHeaderValue);

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }

    /// <summary>
    /// Сохранение истории изменений Подразделения/Должности/Фамилии
    /// </summary>
    /// <param name="newValue">Новое значение</param>
    /// <param name="oldValue">Старое значение</param>
    /// <param name="personId">Id персоны</param>
    /// <param name="db">Контекст БД</param>
    /// <typeparam name="T">Тип объекта, с которым работает метод (Department, JobTitle, Person).</typeparam>
    private async Task SetChanges<T>(T newValue, T oldValue, int personId, ApplicationDbContext db)
    {
        var item = new PersonChange();
        
        var type = typeof(T);
        switch (type.Name)
        {
            case "Department":
                item.Type = ChangeType.Department;
                item.OldValue = (oldValue as Department)?.Name ?? "-";
                item.NewValue = (newValue as Department)?.Name ?? "-";
                break;
            case "JobTitle":
                item.Type = ChangeType.JobTitle;
                item.OldValue = (oldValue as JobTitle)?.Name ?? "-";
                item.NewValue = (newValue as JobTitle)?.Name ?? "-";
                break;
            case "Person":
                item.Type = ChangeType.Lastname;
                item.OldValue = (oldValue as Person)?.LastName ?? "-";
                item.NewValue = (newValue as Person)?.LastName ?? "-";
                break;
        }

        item.PersonId = personId;
        
        await db.PersonChanges.AddAsync(item);
    }
}