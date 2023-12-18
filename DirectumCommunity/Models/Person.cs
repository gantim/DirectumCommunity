namespace DirectumCommunity.Models;

public class Person
{
    private DateTimeOffset? dateOfBirth;
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

    public DateTimeOffset? DateOfBirth
    {
        get => dateOfBirth;
        set
        {
            if (value.HasValue)
                dateOfBirth = value.Value.UtcDateTime;
            else
                dateOfBirth = null;
        }
    }

    public string? INILA { get; set; }
    public string? ShortName { get; set; }
    public string? Sex { get; set; }
    public DateTimeOffset? LastBirthdayNotification { get; set; }
    
    public int? CityId { get; set; }
    public City? City { get; set; }

    public void Update(Person person)
    {
        Name = person.Name;
        TIN = person.TIN;
        LegalAddress = person.LegalAddress;
        PostalAddress = person.PostalAddress;
        Phones = person.Phones;
        Email = person.Email;
        Homepage = person.Homepage;
        Note = person.Note;
        Nonresident = person.Nonresident;
        PSRN = person.PSRN;
        NCEO = person.NCEO;
        NCEA = person.NCEA;
        Account = person.Account;
        CanExchange = person.CanExchange;
        Code = person.Code;
        ExternalId = person.ExternalId;
        Status = person.Status;
        LastName = person.LastName;
        FirstName = person.FirstName;
        MiddleName = person.MiddleName;
        DateOfBirth = person.DateOfBirth;
        INILA = person.INILA;
        ShortName = person.ShortName;
        Sex = person.Sex;
        CityId = person.CityId;
    }
}