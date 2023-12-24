using System.Text;
using DirectumCommunity.Models.ViewModels;
using DirectumCommunity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DirectumCommunity.Controllers;

[Authorize]
public class SubstitutionsController : BaseController
{
    private readonly SubstitutionService _substitutionService;
    private readonly ILogger<SubstitutionsController> _logger;

    public SubstitutionsController(ILogger<SubstitutionsController> logger,
        EmployeeService employeeService,
        SubstitutionService substitutionService)
    : base(employeeService)
    {
        _logger = logger;
        _substitutionService = substitutionService;
    }
    
    public async Task<IActionResult> Index()
    {
        var filters = await _substitutionService.GetFilters();
        ViewBag.Title = "Календарь отсутствий";
        return View(filters);
    }

    [HttpPost]
    public async Task<IActionResult> GetAllSubstitutionsInMonth([FromBody]SubstitutionRequest request)
    {
        var substitutions =
            await _substitutionService.GetAllSubstitutionsInMonth(request.Year, request.Month, request.Filter);
        return Json(substitutions);
    }
    
    [HttpPost]
    public async Task<IActionResult> GetAllSubstitutionsInYear([FromBody]SubstitutionRequest request)
    {
        var substitutions =
            await _substitutionService.GetAllSubstitutionsInYear(request.Year, request.Filter);
        return Json(substitutions);
    }

    [HttpPost]
    public FileResult ExportToExcel(string htmlTable)
    {
        return File(Encoding.ASCII.GetBytes(htmlTable), "application/vnd.ms-excel", "test.xlsx");
    }
}