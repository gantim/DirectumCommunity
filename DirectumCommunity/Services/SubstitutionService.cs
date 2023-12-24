using System.Diagnostics;
using DirectumCommunity.Models;
using DirectumCommunity.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DirectumCommunity.Services;

public class SubstitutionService
{
    private readonly EmployeeService _employeeService = new();

    public async Task<List<SubstitutionInYear>> GetAllSubstitutionsInYear(int year, SubstitutionFilter? filter = null)
    {
        var substitutionsInYear = new List<SubstitutionInYear>();
        await using (var db = new ApplicationDbContext())
        {
            var employees = await _employeeService.GetAll();

            if (filter != null)
            {
                employees = employees.Where(e =>
                    (e.Id == filter.EmployeeId || filter.EmployeeId == 0) &&
                    (e.JobTitleId == filter.JobTitleId || filter.JobTitleId == 0) &&
                    (e.DepartmentId == filter.DepartmentId || filter.DepartmentId == 0)).ToList();
            }
            
            foreach (var employee in employees)
            {
                var substitutionInYear = new SubstitutionInYear()
                {
                    Id = employee.Id,
                    Avatar = employee.Avatar,
                    Department = employee.Department?.Name,
                    Name = $"{employee.Person?.LastName} {employee.Person?.FirstName}"
                };

                for (var i = 1; i <= 12; i++)
                {
                    var substitutions = await GetMonthSubstitutionByEmployeeId(employee.Id, year, i);
                    var substitutionsItems = substitutions.Select(substitution => new SubstitutionItem()
                        {
                            SubstituteName =
                                $"{substitution.Substitute?.Person?.LastName} {substitution.Substitute?.Person?.FirstName}",
                            SubstituteDepartment = substitution.Substitute?.Department?.Name,
                            StartDate = substitution.StartDate,
                            EndDate = substitution.EndDate,
                            Reason = substitution.Comment,
                            TypeReason = GetTypeReason(substitution.Comment)
                        })
                        .ToList();

                    substitutionInYear.SubstitutionsMonth.Add(new SubstitutionMonth()
                    {
                        Month = i,
                        Substitutions = substitutionsItems
                    });
                }

                substitutionsInYear.Add(substitutionInYear);
            }
        }

        if (filter != null)
        {
            substitutionsInYear = substitutionsInYear
                .Select(year => new SubstitutionInYear
                {
                    Id = year.Id,
                    Name = year.Name,
                    Avatar = year.Avatar,
                    Department = year.Department,
                    SubstitutionsMonth = year.SubstitutionsMonth
                        .Select(month => new SubstitutionMonth
                        {
                            Month = month.Month,
                            Substitutions = month.Substitutions
                                .Where(item =>
                                    ((filter.IsVacation == 1 ||
                                      (filter.IsVacation == 0 && item.TypeReason == 1)) &&
                                     (filter.IsMedicalLeave == 1 ||
                                      (filter.IsMedicalLeave == 0 && item.TypeReason == 2))))
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();
        }

        return substitutionsInYear;
    }

    public async Task<List<SubstitutionInMonth>> GetAllSubstitutionsInMonth(int year, int month, SubstitutionFilter? filter = null)
    {
        var substitutionsInMonth = new List<SubstitutionInMonth>();
        await using (var db = new ApplicationDbContext())
        {
            var employees = await _employeeService.GetAll();

            if (filter != null)
            {
                employees = employees.Where(e =>
                    (e.Id == filter.EmployeeId || filter.EmployeeId == 0) &&
                    (e.JobTitleId == filter.JobTitleId || filter.JobTitleId == 0) &&
                    (e.DepartmentId == filter.DepartmentId || filter.DepartmentId == 0)).ToList();
            }
            
            foreach (var employee in employees)
            {
                var substitutions = await GetMonthSubstitutionByEmployeeId(employee.Id, year, month);

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

        if (filter != null)
        {
            substitutionsInMonth = substitutionsInMonth
                .Select(substitution => new SubstitutionInMonth
                {
                    Id = substitution.Id,
                    Name = substitution.Name,
                    Avatar = substitution.Avatar,
                    Department = substitution.Department,
                    Substitutions = substitution.Substitutions
                        .Where(item =>
                            ((filter.IsVacation == 1 ||
                              (filter.IsVacation == 0 && item.TypeReason == 1)) &&
                             (filter.IsMedicalLeave == 1 ||
                              (filter.IsMedicalLeave == 0 && item.TypeReason == 2))))
                        .ToList()
                })
                .ToList();
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
    
    public async Task<List<Substitution>> GetMonthSubstitutionByEmployeeId(int id, int year, int month)
    {
        DateTimeOffset start = new DateTimeOffset(new DateTime(year, month, 1), TimeSpan.Zero);
        DateTimeOffset end = new DateTimeOffset(new DateTime(year, month, DateTime.DaysInMonth(year, month)), TimeSpan.Zero).AddDays(1).AddTicks(-1);
    
        await using (var db = new ApplicationDbContext())
        {
            return await db.Substitutions.Include(s => s.User)
                .Include(s => s.Substitute)
                .ThenInclude(s => s!.Person)
                .Include(s => s.Substitute)
                .ThenInclude(s => s!.Department)
                .Where(s => s.UserId == id && s.StartDate <= end && s.EndDate >= start)
                .ToListAsync();
        }
    }

    public async Task<SubstitutionFilters> GetFilters()
    {
        await using (var db = new ApplicationDbContext())
        {
            var filters = new SubstitutionFilters();
            
            var employeesFio = await db.Employees.Select(e => new SelectListItem()
            {
                Value = e.Id.ToString(),
                Text = e.Name
            }).ToListAsync();
            
            var jobTitles = await db.JobTitles.Select(j => new SelectListItem()
            {
                Value = j.Id.ToString(),
                Text = j.Name
            }).ToListAsync();
            
            var departments = await db.Departments.Select(d => new SelectListItem()
            {
                Value = d.Id.ToString(),
                Text = d.Name
            }).ToListAsync();
            
            filters.EmployeesFio.AddRange(employeesFio);
            filters.JobTitles.AddRange(jobTitles);
            filters.Departments.AddRange(departments);

            return filters;
        }
    }
}