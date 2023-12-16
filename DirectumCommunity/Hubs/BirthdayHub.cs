using DirectumCommunity.Extensions;
using DirectumCommunity.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DirectumCommunity.Hubs;

public class BirthdayHub : Hub
{
    public async Task BirthdayNotification(string id)
    {
        if (int.TryParse(id, out int personId))
        {
            using (var db = new ApplicationDbContext())
            {
                var person = await db.Persons.FirstOrDefaultAsync(p => p.Id == personId);
                if (person != null)
                {
                    if (person.IsBirthdayToday() && !person.IsSendBirthdayNotification())
                    {
                        person.LastBirthdayNotification = DateTimeOffset.Now.ToOffset(TimeSpan.Zero);
                        db.SaveChanges();
                        await Clients.Caller.SendAsync("Birthday", true);
                    }
                }
            }
        }
    }
}