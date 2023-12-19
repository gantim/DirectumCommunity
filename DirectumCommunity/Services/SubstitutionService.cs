using System.Diagnostics;
using DirectumCommunity.Models;
using DirectumCommunity.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DirectumCommunity.Services;

public class SubstitutionService
{
    private readonly EmployeeService _employeeService = new();

    public async Task<List<SubstitutionInMonth>> GetAllSubstitutionsInMonth(int year, int month)
    {
        var substitutionsInMonth = new List<SubstitutionInMonth>();
        await using (var db = new ApplicationDbContext())
        {
            var employees = await _employeeService.GetAll();

            foreach (var employee in employees)
            {
                var substitutions = await GetSubstitutionByEmployeeId(employee.Id, year, month);
                
                var substitutionInMonth = new SubstitutionInMonth()
                {
                    Id = employee.Id,
                    Avatar = employee.Avatar,
                    Department = employee.Department?.Name,
                    Name = $"{employee.Person?.LastName} {employee.Person?.FirstName}"
                };
                
                foreach (var substitution in substitutions)
                {
                    substitutionInMonth.Substitutions.Add(new SubstitutionItem()
                    {
                        SubstituteName =
                            $"{substitution.Substitute?.Person?.LastName} {substitution.Substitute?.Person?.FirstName}",
                        SubstituteDepartment = substitution.Substitute?.Department?.Name,
                        StartDate = substitution.StartDate,
                        EndDate = substitution.EndDate,
                        Reason = substitution.Comment,
                        TypeReason = GetTypeReason(substitution.Comment)
                    });
                }
                
                substitutionsInMonth.Add(substitutionInMonth);
            }
        }

        return substitutionsInMonth;
    }

    private int GetTypeReason(string? reason)
    {
        if (!string.IsNullOrEmpty(reason))
        {
            if (reason.ToLower().Contains("б/л"))
            {
                return 1;
            }

            if(reason.ToLower().Contains("отпуск"))
            {
                return 2;
            }
        }

        return 0;
    }
    
    public async Task<List<Substitution>> GetSubstitutionByEmployeeId(int id, int year, int month)
    {
        await using (var db = new ApplicationDbContext())
        {
            return await db.Substitutions.Include(s => s.User)
                .Include(s => s.Substitute)
                .ThenInclude(s => s!.Person)
                .Include(s => s.Substitute)
                .ThenInclude(s => s!.Department)
                .Where(s => s.StartDate != null && s.UserId == id && s.StartDate.Value.Year == year && s.StartDate.Value.Month == month )
                .ToListAsync();
        }
    }
}