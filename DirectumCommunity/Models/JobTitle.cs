namespace DirectumCommunity.Models;

public class JobTitle
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ExternalId { get; set; }
    public string? Status { get; set; }

    public void Update(JobTitle jobTitle)
    {
        Name = jobTitle.Name;
        ExternalId = jobTitle.ExternalId;
        Status = jobTitle.Status;
    }
}