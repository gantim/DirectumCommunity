using Microsoft.AspNetCore.Mvc.Rendering;

namespace DirectumCommunity.Models.ViewModels;

public class SubstitutionFilters
{
    public List<SelectListItem> EmployeesFio { get; set; } = new();
    public List<SelectListItem> JobTitles { get; set; } = new();
    public List<SelectListItem> Departments { get; set; } = new();
}