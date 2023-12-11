using DirectumCommunity.Models;
using Microsoft.EntityFrameworkCore;

namespace DirectumCommunity.Services;

public class EmployeeService
{
    public async Task<List<Employee>> GetAll()
    {
        await using (var db = new ApplicationDbContext())
        {
            return await db.Employees.Include(e => e.Department)
                .Include(e => e.JobTitle)
                .Include(e => e.Person)
                .Include(l => l.Login)
                .ToListAsync();
        }
    }
    
    public async Task<Employee?> GetEmployeeByLogin(string login)
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

    public async Task<string> GetPhotoByEmployeeId(int id){
        if(id != 0){
            await using (var db = new ApplicationDbContext())
        {
            var employee = await db.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if(employee != null){
                var photo = await db.PersonalPhotos.FirstOrDefaultAsync(pp => pp.PersonalPhotoHash == employee.PersonalPhotoHash);
                if(photo != null){
                    return Convert.ToBase64String(photo.Value);
                }
            }
        }
        }

        return string.Empty;
    }
}