namespace DirectumCommunity.Models;

public class Employee
{
    public int Id { get; set; }
    public string Sid { get; set; }
    public bool? IsSystem { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string PersonalPhotoHash { get; set; }
    public string Phone { get; set; }
    public string Note { get; set; }
    public string Email { get; set; }
    public bool NeedNotifyExpiredAssignments { get; set; }
    public bool NeedNotifyNewAssignments { get; set; }
    public string PersonnelNumber { get; set; }
    public bool NeedNotifyAssignmentsSummary { get; set; }
    public string ExternalId { get; set; }
}