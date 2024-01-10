using DirectumCommunity.Models;
using Microsoft.EntityFrameworkCore;

namespace DirectumCommunity.Services;

public class NotificationService
{
    public async Task<int> GetNotificationsCount(int employeeId)
    {
        await using (var db = new ApplicationDbContext())
        {
            var count = await db.NotificationReads
                .Where(n => n.EmployeeId == employeeId && !n.IsRead).CountAsync();

            return count;
        }
    }
    
    public async Task ReadNotifications(int employeeId)
    {
        await using (var db = new ApplicationDbContext())
        {
            var notifications = db.NotificationReads.Where(nr => nr.EmployeeId == employeeId);

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            await db.SaveChangesAsync();
        }
    }

    public async Task<List<Notification>> GetNotifications(int employeeId)
    {
        await using (var db = new ApplicationDbContext())
        {
            return await db.NotificationReads.Include(nr => nr.Notification)
                .Where(n => n.EmployeeId == employeeId && !n.IsRead).Select(n => n.Notification).ToListAsync();
        }
    }

    public async Task AddNotification(string message)
    {
        await using (var db = new ApplicationDbContext())
        {
            var lastId = await GetLastNotificationId();
            var notificationId = lastId == 0 ? 1 : lastId + 1;
            
            var notification = new Notification()
            {
                Id = notificationId,
                Message = message
            };
            
            db.Notifications.Add(notification);

            await AddNotificationsToAllEmployees(db, notification);

            await db.SaveChangesAsync();
        }
    }

    private async Task AddNotificationsToAllEmployees(ApplicationDbContext db, Notification notification)
    {
        var employeesIds = await db.Employees.Select(e => e.Id).ToListAsync();

        foreach (var id in employeesIds)
        {
            db.NotificationReads.Add(new NotificationRead()
            {
                NotificationId = notification.Id,
                EmployeeId = id,
                IsRead = false
            });
        }
    }
    
    private async Task<int> GetLastNotificationId()
    {
        await using (var db = new ApplicationDbContext())
        {
            return await db.Notifications.MaxAsync(n => (int?)n.Id) ?? 1;
        }
    }
}