using System.Text.Json;
using System.Text.Json.Serialization;
using DirectumCommunity.Models;
using DirectumCommunity.Models.ViewModels;
using DirectumCommunity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DirectumCommunity.Controllers;

[Authorize]
public class EventsCalendarController : BaseController
{
    private readonly MeetingService _meetingService;
    public EventsCalendarController(MeetingService meetingService, EmployeeService employeeService) 
        : base(employeeService)
    {
        _meetingService = new MeetingService();
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        await GetNavbarData();
        ViewBag.Title = "Календарь событий";
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetMeetings([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var meetings = await _meetingService.GetMeetings(start, end);
        return Json(meetings);
    }
}