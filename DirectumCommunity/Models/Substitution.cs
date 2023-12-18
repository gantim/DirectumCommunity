namespace DirectumCommunity.Models;

public class Substitution
{
    public int Id { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string? Name { get; set; }
    public bool? IsSystem { get; set; }
    public bool? DelegateStrictRights { get; set; }
    public string? Comment { get; set; }
    public string? Status { get; set; }
    
    public int? UserId { get; set; }
    public Employee? User { get; set; }
    
    public int? SubstituteId { get; set; }
    public Employee? Substitute { get; set; }
    
    public void Update(Substitution substitution)
    {
        StartDate = substitution.StartDate;
        EndDate = substitution.EndDate;
        Name = substitution.Name;
        IsSystem = substitution.IsSystem;
        DelegateStrictRights = substitution.DelegateStrictRights;
        Comment = substitution.Comment;
        Status = substitution.Status;
        UserId = substitution.UserId;
        SubstituteId = substitution.SubstituteId;
    }
}