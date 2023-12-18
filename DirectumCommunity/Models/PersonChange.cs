using System.ComponentModel.DataAnnotations;

namespace DirectumCommunity.Models;

public class PersonChange
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public Person Person { get; set; }
    [EnumDataType(typeof(ChangeType))]
    public ChangeType Type { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public DateTimeOffset ModifyDate { get; set; } = DateTimeOffset.Now.ToOffset(TimeSpan.Zero);
}