namespace DirectumCommunity.Models.ViewModels;

public class SubstitutionInYear
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Avatar { get; set; }
    public string? Department { get; set; }
    public List<SubstitutionMonth> SubstitutionsMonth { get; set; } = new();
}

public class SubstitutionMonth
{
    public int Month { get; set; }
    public List<SubstitutionItem> Substitutions { get; set; } = new();
}