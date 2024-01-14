namespace DirectumCommunity.Models.ViewModels;

public class SubstitutionInMonth
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Avatar { get; set; }
    public string? Department { get; set; }
    public string? BirthDay { get; set; }
    public List<SubstitutionItem> Substitutions { get; set; } = new();
}