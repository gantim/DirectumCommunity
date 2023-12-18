using DirectumCommunity.Models;
using DirectumCommunity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DirectumCommunity.Controllers;

[Authorize]
public class EmployeesController : BaseController
{
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(ILogger<EmployeesController> logger,
        EmployeeService employeeService)
    : base(employeeService)
    {
        _logger = logger;
    }
    
    public async Task<IActionResult> Index()
    {
        var list = await EmployeeService.GetAll();
        ViewBag.Title = "Наши сотрудники";
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployeeInfo(int id)
    {
        var employee = await EmployeeService.GetByIdWithChanges(id);

        return PartialView("EmployeeInfoModal", employee);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetBirthdayNotification(int id)
    {
        var employee = await EmployeeService.GetById(id);

        return PartialView("BirthdayNotification", $"{employee?.Person.FirstName} {employee?.Person.MiddleName}");
    }
}