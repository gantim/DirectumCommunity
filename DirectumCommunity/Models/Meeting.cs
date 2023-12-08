namespace DirectumCommunity.Models;

public class Meeting
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string Note { get; set; }
    public string DisplayName { get; set; }
    public double Duration { get; set; }
    public string Status { get; set; }
    public DateTime DateTime { get; set; }
}