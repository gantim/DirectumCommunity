namespace DirectumCommunity.Models;

public class MeetingMember
{
    public int EmployeesId { get; set; }
    public Employee Employees { get; set; }
    public int MeetingsId { get; set; }
    public Meeting Meetings { get; set; }
}