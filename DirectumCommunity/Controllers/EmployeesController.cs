using DirectumCommunity.Models;
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

        Task.Run(async () => await GetNavbarData());
    }
    
    public async Task<IActionResult> Index()
    {
        var list = await _employeeService.GetAll();
        ViewBag.Title = "Наши сотрудники";
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployeeInfo(int id)
    {
        var employee = await _employeeService.GetById(id);

        return PartialView("EmployeeInfoModal", employee);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetBirthdayNotification(int id)
    {
        var employee = await _employeeService.GetById(id);

        return PartialView("BirthdayNotification", $"{employee?.Person.FirstName} {employee?.Person.MiddleName}");
    }
    
    private async Task GetNavbarData()
    {
        var login = User.Identity?.Name;
        var navbarData = await _employeeService.GetNavbarDataByLogin(login);
        ViewBag.UserName = navbarData.Name;
        ViewBag.Photo = navbarData.Avatar;
        ViewBag.Initials = navbarData.Initials;
        ViewBag.EmployeeId = navbarData.EmployeeId;
        ViewBag.PersonId = navbarData.PersonId;
    }
}