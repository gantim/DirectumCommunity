using DirectumCommunity.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectumCommunity.Controllers;

public class BaseController : Controller
{
    public readonly EmployeeService EmployeeService;
    
    public BaseController(EmployeeService employeeService)
    {
        EmployeeService = employeeService;
    }
    
    public async Task GetNavbarData()
    {
        var login = User.Identity?.Name;
        var navbarData = await EmployeeService.GetNavbarDataByLogin(login);
        ViewBag.UserName = navbarData.Name;
        ViewBag.Photo = navbarData.Avatar;
        ViewBag.Initials = navbarData.Initials;
        ViewBag.EmployeeId = navbarData.EmployeeId;
        ViewBag.PersonId = navbarData.PersonId;
    }
}