namespace DirectumCommunity.Models;

public class NotificationRead
{
    public int Id { get; set; }
    public int NotificationId { get; set; }
    public Notification Notification { get; set; }
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public bool IsRead { get; set; } = false;
}