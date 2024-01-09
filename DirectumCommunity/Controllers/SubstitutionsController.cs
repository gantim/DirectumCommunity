using System.Text;
using DirectumCommunity.Models.ViewModels;
using DirectumCommunity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace DirectumCommunity.Controllers;

[Authorize]
public class SubstitutionsController : BaseController
{
    private readonly SubstitutionService _substitutionService;
    private readonly ExcelService _excelService;

    public SubstitutionsController(ILogger<SubstitutionsController> logger,
        EmployeeService employeeService,
        SubstitutionService substitutionService)
    : base(employeeService)
    {
        _substitutionService = substitutionService;
        _excelService = new ExcelService();
    }
    
    public async Task<IActionResult> Index()
    {
        await GetNavbarData();
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
    public async Task<FileResult> ExportToExcel([FromBody] SubstitutionRequest request)
    {
        var list = await _substitutionService.GetAllSubstitutionsInMonth(request.Year, request.Month, request.Filter);
        var excel = _excelService.CreateSubstitutionInMonth(list, request.Year, request.Month);
        return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "substitutions.xls");
    }
}