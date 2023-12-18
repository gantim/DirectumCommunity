namespace DirectumCommunity.Models;

public class Department
{
    public int Id { get; set; }
    public string? Sid { get; set; }
    public bool? IsSystem { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? Phone { get; set; }
    public string? ShortName { get; set; }
    public string? Note { get; set; }
    public string? Code { get; set; }
    public string? ExternalId { get; set; }

    public void Update(Department department)
    {
        Sid = department.Sid;
        IsSystem = department.IsSystem;
        Name = department.Name;
        Description = department.Description;
        Status = department.Status;
        Phone = department.Phone;
        ShortName = department.ShortName;
        Note = department.Note;
        Code = department.Code;
        ExternalId = department.ExternalId;
    }
}