using DirectumCommunity.Models;
using DirectumCommunity.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectumCommunity.Controllers;

public class NotificationController : BaseController
{
    public NotificationController(EmployeeService employeeService, NotificationService notificationService)
        : base(employeeService, notificationService) { }

    [HttpGet]
    public async Task<IActionResult> GetNotifications(int id)
    {
        var notifications = await NotificationService.GetNotifications(id);
        return PartialView("NotificationsModal", notifications);
    }
    
    [HttpPost]
    public async Task<IActionResult> MakeAsRead(int id)
    {
        try
        {
            await NotificationService.ReadNotifications(id);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new { Error = e.Message });
        }
    }
}