using System.ComponentModel.DataAnnotations.Schema;

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

    public Department? Department { get; set; }
    public JobTitle? JobTitle { get; set; }
    public Person? Person { get; set; }
    public Login? Login { get; set; }
    public PersonalPhoto? PersonalPhoto { get; set; }
    
    [NotMapped]
    public string Avatar { get; set; }

    [NotMapped]
    public Organization? Organization { get; set; }
    public DateTimeOffset? CreateDate { get; set; }
    
    public override string ToString()
    {
        return $"{Name}";
    }

    public void Update(Employee employee)
    {
        Sid = employee.Sid;
        IsSystem = employee.IsSystem;
        Name = employee.Name;
        Description = employee.Description;
        Status = employee.Status;
        PersonalPhotoHash = employee.PersonalPhotoHash;
        Phone = employee.Phone;
        Note = employee.Note;
        Email = employee.Email;
        NeedNotifyExpiredAssignments = employee.NeedNotifyExpiredAssignments;
        NeedNotifyNewAssignments = employee.NeedNotifyNewAssignments;
        NeedNotifyAssignmentsSummary = employee.NeedNotifyAssignmentsSummary;
        PersonnelNumber = employee.PersonnelNumber;
        ExternalId = employee.ExternalId;
        DepartmentId = employee.DepartmentId;
        JobTitleId = employee.JobTitleId;
        PersonId = employee.PersonId;
        LoginId = employee.LoginId;
        LastModifyDate = employee.LastModifyDate;
    }
}