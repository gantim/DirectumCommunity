namespace DirectumCommunity.Models;

public class Meeting
{
    private DateTimeOffset? dateTime;
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    public string? Note { get; set; }
    public string? DisplayName { get; set; }
    public double? Duration { get; set; }
    public string? Status { get; set; }
    public int? SecretaryId { get; set; }
    public Employee? Secretary { get; set; }
    public int? PresidentId { get; set; }
    public Employee? President { get; set; }
    
    public DateTimeOffset? DateTime
    {
        get => dateTime;
        set
        {
            if (value.HasValue)
                dateTime = value.Value.UtcDateTime;
            else
                dateTime = null;
        }
    }

    public List<Employee> Employees { get; set; } = new();
    public List<MeetingMember> MeetingMembers { get; set; } = new();
    
    public void Update(Meeting meetingModel)
    {
        Name = meetingModel.Name;
        Location = meetingModel.Location;
        Note = meetingModel.Note;
        DisplayName = meetingModel.DisplayName;
        Duration = meetingModel.Duration;
        Status = meetingModel.Status;
        SecretaryId = meetingModel.SecretaryId;
        PresidentId = meetingModel.PresidentId;
        DateTime = meetingModel.DateTime;
    }
}