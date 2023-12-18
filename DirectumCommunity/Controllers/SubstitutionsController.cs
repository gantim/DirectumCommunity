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
        ViewBag.Title = "Календарь отсутствий";
        return View();
    }
}