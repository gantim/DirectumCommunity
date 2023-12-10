namespace DirectumCommunity.Models;

public class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? TIN { get; set; }
    public string? LegalAddress { get; set; }
    public string? PostalAddress { get; set; }
    public string? Phones { get; set; }
    public string? Email { get; set; }
    public string? Homepage { get; set; }
    public string? Note { get; set; }
    public bool? Nonresident { get; set; }
    public string? PSRN { get; set; }
    public string? NCEO { get; set; }
    public string? NCEA { get; set; }
    public string? Account { get; set; }
    public bool? CanExchange { get; set; }
    public string? Code { get; set; }
    public string? ExternalId { get; set; }
    public string? Status { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    private DateTimeOffset? dateOfBirth;
    public DateTimeOffset? DateOfBirth
    {
        get { return dateOfBirth; }
        set
        {
            if (value.HasValue)
            {
                dateOfBirth = value.Value.UtcDateTime;
            }
            else
            {
                dateOfBirth = null;
            }
        }
    }
    public string? INILA { get; set; }
    public string? ShortName { get; set; }
    public string? Sex { get; set; }
}