namespace DirectumCommunity.Models.Responses;

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public object TIN { get; set; }
    public object LegalAddress { get; set; }
    public object PostalAddress { get; set; }
    public object Phones { get; set; }
    public object Email { get; set; }
    public object Homepage { get; set; }
    public object Note { get; set; }
    public bool Nonresident { get; set; }
    public object PSRN { get; set; }
    public object NCEO { get; set; }
    public object NCEA { get; set; }
    public object Account { get; set; }
    public bool CanExchange { get; set; }
    public object Code { get; set; }
    public object ExternalId { get; set; }
    public string Status { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public object INILA { get; set; }
    public string ShortName { get; set; }
    public string Sex { get; set; }
}