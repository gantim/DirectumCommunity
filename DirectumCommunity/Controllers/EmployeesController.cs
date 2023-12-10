using DirectumCommunity.Models;
using DirectumCommunity.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectumCommunity.Controllers;

public class EmployeesController : Controller
{
    private readonly ILogger<EmployeesController> _logger;
    private readonly IDirectumService _directumService;
    private readonly EmployeeService _employeeService;

    public EmployeesController(IDirectumService directumService,
        ILogger<EmployeesController> logger,
        EmployeeService employeeService)
    {
        _directumService = directumService;
        _logger = logger;
        _employeeService = employeeService;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _employeeService.GetAll();
        ViewBag.Title = "Наши сотрудники";
        return View(list);
    }
}