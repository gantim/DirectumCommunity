namespace DirectumCommunity.Models.ViewModels;

public class SubstitutionItem
{
    public string? SubstituteName { get; set; }
    public string? SubstituteDepartment { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string? Reason { get; set; }
    public int? TypeReason { get; set; }
}