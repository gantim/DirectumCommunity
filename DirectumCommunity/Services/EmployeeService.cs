using DirectumCommunity.Models;
using Microsoft.EntityFrameworkCore;

namespace DirectumCommunity.Services;

public class EmployeeService
{
    public async Task<List<Employee>> GetAll()
    {
        await using (var db = new ApplicationDbContext())
        {
            return await db.Employees.Include(e => e.Department).Include(e => e.JobTitle).Include(e => e.Person).ToListAsync();
        }
    }
}