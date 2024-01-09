using System.Text;
using DirectumCommunity.Models;
using DirectumCommunity.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DirectumCommunity.Services;

public class MeetingService
{
    public async Task<List<CalendarItemModel>> GetMeetings(DateTime startDate, DateTime endDate)
    {
        await using (var db = new ApplicationDbContext())
        {
            DateTimeOffset start = new DateTimeOffset(DateTime.SpecifyKind(startDate, DateTimeKind.Utc));
            DateTimeOffset end = new DateTimeOffset(DateTime.SpecifyKind(endDate, DateTimeKind.Utc));
            
            var meetings = await db.Meetings
                .Include(m => m.President)
                .ThenInclude(s => s!.Person)
                .Include(m => m.Secretary)
                .ThenInclude(s => s!.Person)
                .Include(m => m.Employees)
                .ThenInclude(e => e.Person)
                .Where(m => m.DateTime <= end && m.DateTime >= start)
                .ToListAsync();

            var result = new List<CalendarItemModel>();

            foreach (var meeting in meetings)
            {
                result.Add(ToCalendarItemModel(meeting));
            }
            
            return result;
        }
    }

    private CalendarItemModel ToCalendarItemModel(Meeting meeting)
    {
        var model = new CalendarItemModel();
        model.Id = meeting.Id.ToString();
        model.Title = meeting.Name;
        model.Start = meeting.DateTime.Value.ToString("yyyy-MM-ddTHH:mm:ss");
        model.End = meeting.DateTime.Value.AddHours(meeting.Duration.Value).ToString("yyyy-MM-ddTHH:mm:ss");
        model.Description = GenerateDescription(meeting);
        model.Members = GenerateMembersList(meeting);
        return model;
    }

    private string GenerateMembersList(Meeting meeting)
    {
        var members = meeting.Employees.Select(p => p.Person.ShortName).ToList();
        StringBuilder html = new StringBuilder();
        html.Append("<div class=\"container\">");

        foreach (var member in members)
        {
            html.Append("<div class=\"row\">");
            html.Append(member);
            html.Append("</div>");
        }
        
        html.Append("</div>");

        return html.ToString();
    }
    
    private string GenerateDescription(Meeting meeting)
    {
        string html = @$"<div class=""container"">
                        <div class=""row"">
                            <div class=""col"">
                                <span class=""events-calendar-popover-field-name"">Начало:</span>
                            </div>
                            <div class=""col"">
                                <span>{meeting.DateTime.Value:HH:mm}</span>
                            </div>
                        </div>
                        <div class=""row"">
                            <div class=""col"">
                                <span class=""events-calendar-popover-field-name"">Окончание:</span>
                            </div>
                            <div class=""col"">
                                <span>{meeting.DateTime.Value.AddHours(meeting.Duration.Value):HH:mm}</span>
                            </div>
                        </div>
                        <div class=""row"">
                            <div class=""col"">
                                <span class=""events-calendar-popover-field-name"">Место:</span>
                            </div>
                            <div class=""col"">
                                <span>{meeting.Location}</span>
                            </div>
                        </div>
                        <div class=""row"">
                            <div class=""col"">
                                <span class=""events-calendar-popover-field-name"">Секретарь:</span>
                            </div>
                            <div class=""col"">
                                <span>{meeting.Secretary.Person.ShortName}</span>
                            </div>
                        </div>
                        <div class=""row"">
                            <div class=""col"">
                                <span class=""events-calendar-popover-field-name"">Председатель:</span>
                            </div>
                            <div class=""col"">
                                <span>{meeting.President.Person.ShortName}</span>
                            </div>
                        </div>
                        <div class=""row"">
                            <div class=""col"">
                                <span class=""events-calendar-popover-field-name"">Участники:</span>
                            </div>
                            <div class=""col"">
                                <span id=""pop-{meeting.Id}"" class=""popover-link"">Посмотреть всех</span>
                            </div>
                        </div>
                        <div class=""row"">
                            <div class=""col"">
                                <span class=""events-calendar-popover-field-name"">Примечание:</span>
                            </div>
                            <div class=""col"">
                                <span>{meeting.Note}</span>
                            </div>
                        </div>
                    </div>";

        return html;
    }
}