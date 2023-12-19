namespace DirectumCommunity.Models.ViewModels;

public class SubstitutionRequest
{
    public int Year { get; set; }
    public int Month { get; set; }
    public SubstitutionFilter Filter { get; set; }
}