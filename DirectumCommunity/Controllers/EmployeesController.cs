using DirectumCommunity.Models;
using DirectumCommunity.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectumCommunity.Controllers;

public class EmployeesController : Controller
{
    private readonly ILogger<EmployeesController> _logger;
    private readonly IDirectumService _directumService;

    public EmployeesController(IDirectumService directumService,
        ILogger<EmployeesController> logger)
    {
        _directumService = directumService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Title = "Наши сотрудники";
        return View();
    }
}