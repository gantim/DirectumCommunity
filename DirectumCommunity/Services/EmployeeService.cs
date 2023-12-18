using DirectumCommunity.Models;
using DirectumCommunity.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DirectumCommunity.Services;

public class EmployeeService
{
    public async Task<List<Employee>> GetAll()
    {
        await using (var db = new ApplicationDbContext())
        {
            var employees = await db.Employees.Include(e => e.Department)
                .Include(e => e.JobTitle)
                .Include(e => e.Person)
                .Include(l => l.Login)
                .ToListAsync();

            foreach (var employee in employees)
            {
                employee.Avatar = await GetEmployeePhoto(employee);
            }

            return employees;
        }
    }

    public async Task<EmployeeInfoViewModel?> GetByIdWithChanges(int id)
    {
        await using (var db = new ApplicationDbContext())
        {
            var employeeInfoViewModel = new EmployeeInfoViewModel();
            
            var employee = await db.Employees.Include(e => e.Department)
                .Include(e => e.JobTitle)
                .Include(l => l.Login)
                .Include(e => e.Person)
                .ThenInclude(c => c!.City)
                .FirstOrDefaultAsync(e => e.Id == id);
            
            if (employee != null)
            {
                employee.Avatar = await GetEmployeePhoto(employee);
                employee.Organization = await db.Organizations.FirstOrDefaultAsync();
                
                employeeInfoViewModel.Employee = employee;
                employeeInfoViewModel.History = db.PersonChanges.Where(pc => pc.PersonId == employee.PersonId).ToList();
                employeeInfoViewModel.FillChanges();
            }
            
            return employeeInfoViewModel;
        }
    }
    
    public async Task<Employee?> GetById(int id)
    {
        await using (var db = new ApplicationDbContext())
        {
            var employee = await db.Employees.Include(e => e.Department)
                .Include(e => e.JobTitle)
                .Include(l => l.Login)
                .Include(e => e.Person)
                .ThenInclude(c => c!.City)
                .FirstOrDefaultAsync(e => e.Id == id);
            
            if (employee != null)
            {
                employee.Avatar = await GetEmployeePhoto(employee);
                employee.Organization = await db.Organizations.FirstOrDefaultAsync();
            }

            return employee;
        }
    }
    
    public async Task<Employee?> GetByLogin(string login)
    {
        await using (var db = new ApplicationDbContext())
        {
            return await db.Employees.Include(e => e.Department)
                .Include(e => e.JobTitle)
                .Include(e => e.Person)
                .Include(l => l.Login)
                .FirstOrDefaultAsync(e => e.Login.LoginName == login);
        }
    }

    private async Task<string> GetEmployeePhoto(Employee? employee)
    {
        if (employee != null)
        {
            await using (var db = new ApplicationDbContext())
            {
                var photo = await db.PersonalPhotos.FirstOrDefaultAsync(pp =>
                    pp.PersonalPhotoHash == employee.PersonalPhotoHash);
                if (photo != null)
                {
                    return Convert.ToBase64String(photo.Value);
                }
            }
        }

        return string.Empty;
    }

    public async Task<NavbarData> GetNavbarDataByLogin(string login)
    {
        var employee = await GetByLogin(login);
        var userName = $"{employee?.Person.FirstName} {employee?.Person.LastName}";
        var avatar = await GetEmployeePhoto(employee);
        var initials = $"{employee.Person.FirstName?.FirstOrDefault()}{employee.Person.LastName?.FirstOrDefault()}";

        return new NavbarData()
        {
            EmployeeId = employee.Id,
            PersonId = employee.Person.Id,
            Name = userName,
            Avatar = avatar,
            Initials = initials
        };
    }
}