using DirectumCommunity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DirectumCommunity.Controllers;

[Authorize]
public class EmployeesController : Controller
{
    private readonly IDirectumService _directumService;
    private readonly EmployeeService _employeeService;
    private readonly ILogger<EmployeesController> _logger;

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
        //await _directumService.ImportData();
        var list = await _employeeService.GetAll();
        ViewBag.Title = "Наши сотрудники";
        var login = User.Identity?.Name;
        var employee = await _employeeService.GetEmployeeByLogin(login);
        ViewBag.UserName = $"{employee?.Person.FirstName} {employee?.Person.LastName}";
        ViewBag.Photo = await _employeeService.GetPhotoByEmployeeId(employee.Id);
        ViewBag.Initials = $"{employee.Person.FirstName?.FirstOrDefault()}{employee.Person.LastName?.FirstOrDefault()}";
        return View(list);
    }
}