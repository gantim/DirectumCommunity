using Microsoft.AspNetCore.Mvc;
using DirectumCommunity.Data.Models;

public class WorkersController : Controller
{
    public IActionResult Index() {
        var workers = GetWorkersFromDatabase();

        return View(workers);
    }
    private List<Worker> GetWorkersFromDatabase() {
        // Здесь вы можете использовать логику для получения данных из базы данных
        // Замените этот код на свою логику получения данных
        var workers = new List<Worker>
        {
            new Worker { /* Заполните данными первого сотрудника */ },
            new Worker { /* Заполните данными второго сотрудника */ },
            // ...
        };

        return workers;
    }
}
