using DirectumCommunity.Models;
using DirectumCommunity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DirectumCommunity.Controllers;

[Authorize]
public class EventsCalendar : BaseController
{
    public EventsCalendar(EmployeeService employeeService) : base(employeeService)
    {
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        await GetNavbarData();
        ViewBag.Title = "Календарь событий";
        return View();
    }
}