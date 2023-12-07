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
        var list = await _directumService.GetAllPersons();
        /*var list = new List<Employee>()
        {
            new ()
            {
                LastName = "Соловьева",
                FirstName = "Елена",
                Patronymic = "Олеговна",
                Department = "Отдел продаж",
                Position = "менеджер по продажам"
            },
            new ()
            {
                LastName = "Свешников",
                FirstName = "Ярослав",
                Patronymic = "Глебович",
                Department = "Отдел продаж",
                Position = "менеджер по продажам"
            },
            new ()
            {
                LastName = "Дроздов",
                FirstName = "Олег",
                Patronymic = "Викторович",
                Department = "Отдел продаж",
                Position = "менеджер по продажам"
            },
            new ()
            {
                LastName = "Голубева",
                FirstName = "Ксения",
                Patronymic = "Валерьевна",
                Department = "Отдел продаж",
                Position = "менеджер по продажам"
            },
            new ()
            {
                LastName = "Смирнова",
                FirstName = "Полина",
                Patronymic = "Артемовна",
                Department = "Отдел продаж",
                Position = "менеджер по продажам"
            },
            new ()
            {
                LastName = "Никитин",
                FirstName = "Владимир",
                Patronymic = "Анатольевич",
                Department = "Отдел продаж",
                Position = "менеджер по продажам"
            },
        };*/
        ViewBag.Title = "Наши сотрудники";
        return View(list);
    }
}