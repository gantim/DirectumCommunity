using DirectumCommunity.Models.Responses;

namespace DirectumCommunity.Models;

public class MeetingModel
{
    private DateTimeOffset? dateTime;
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    public string? Note { get; set; }
    public string? DisplayName { get; set; }
    public double? Duration { get; set; }
    public string? Status { get; set; }
    public Employee? Secretary { get; set; }
    public Employee? President { get; set; }
    
    private int? _secretaryId;
    public int? SecretaryId 
    { 
        get => _secretaryId;
        set => _secretaryId = Secretary?.Id ?? value; 
    }

    private int? _presidentId;
    public int? PresidentId 
    { 
        get => _presidentId;
        set => _presidentId = President?.Id ?? value; 
    }
    
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
    
    public List<Employee> Employees { get; set; }
    public List<MemberResponse> Members { get; set; } = new();
    
    public List<int> GetAllEmployeesIds()
    {
        var ids = new List<int>();

        if (SecretaryId != null) ids.Add(SecretaryId.Value);
        if (PresidentId != null) ids.Add(PresidentId.Value);
        if (Members.Any())
        {
            var memberIds = Members.Select(m => m.Member.Id);
            ids.AddRange(memberIds);
        }

        return ids;
    }
}