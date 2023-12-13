namespace DirectumCommunity.Models;

public class Employee
{
    private DateTimeOffset? lastModifyDate;
    public int Id { get; set; }
    public string? Sid { get; set; }
    public bool? IsSystem { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? PersonalPhotoHash { get; set; }
    public string? Phone { get; set; }
    public string? Note { get; set; }
    public string? Email { get; set; }
    public bool? NeedNotifyExpiredAssignments { get; set; }
    public bool? NeedNotifyNewAssignments { get; set; }
    public bool? NeedNotifyAssignmentsSummary { get; set; }
    public string? PersonnelNumber { get; set; }
    public string? ExternalId { get; set; }
    public int? DepartmentId { get; set; }
    public int? JobTitleId { get; set; }
    public int? PersonId { get; set; }
    public int? LoginId { get; set; }

    public DateTimeOffset? LastModifyDate
    {
        get => lastModifyDate;
        set
        {
            if (value.HasValue)
                lastModifyDate = value.Value.UtcDateTime;
            else
                lastModifyDate = null;
        }
    }

    public Department Department { get; set; }
    public JobTitle JobTitle { get; set; }
    public Person Person { get; set; }
    public Login Login { get; set; }
    public PersonalPhoto PersonalPhoto { get; set; }

    public override string ToString()
    {
        return $"{Name}";
    }
}