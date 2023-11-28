using DirectumCommunity.Data.Models;
using DirectumCommunity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;

namespace DirectumCommunity.Controllers
{
    public class DirectumController : Controller
    {
        private readonly ILogger<DirectumController> _logger;

        public DirectumController(ILogger<DirectumController> logger) {
            _logger = logger;
        }
        public IActionResult Index() {
            return View();
        }
        public IActionResult Privacy() {
            return View();
        }

        public IActionResult Workers() {
            var workers = GetWorkersFromDatabase();
            return View(workers);
        }
        private List<Worker> GetWorkersFromDatabase() {
            var workers = new List<Worker> {
                new Worker { birthday = "erw" },
                new Worker { birthday = "123412" },
            };
            return workers;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}