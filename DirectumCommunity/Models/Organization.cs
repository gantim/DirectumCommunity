namespace DirectumCommunity.Models;

public class Organization
{
    public int Id { get; set; }
    public string? Sid { get; set; }
    public bool? IsSystem { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? TIN { get; set; }
    public string? TRRC { get; set; }
    public string? Phones { get; set; }
    public string? LegalName { get; set; }
    public string? LegalAddress { get; set; }
    public string? PostalAddress { get; set; }
    public string? Note { get; set; }
    public string? Email { get; set; }
    public string? Homepage { get; set; }
    public string? PSRN { get; set; }
    public string? NCEO { get; set; }
    public string? NCEA { get; set; }
    public string? Account { get; set; }
    public string? Code { get; set; }
    public bool? Nonresident { get; set; }
    public string? ExternalId { get; set; }
    
    public void Update(Organization organization)
    {
        Sid = organization.Sid;
        IsSystem = organization.IsSystem;
        Name = organization.Name;
        Description = organization.Description;
        Status = organization.Status;
        TIN = organization.TIN;
        TRRC = organization.TRRC;
        Phones = organization.Phones;
        LegalName = organization.LegalName;
        LegalAddress = organization.LegalAddress;
        PostalAddress = organization.PostalAddress;
        Note = organization.Note;
        Email = organization.Email;
        Homepage = organization.Homepage;
        PSRN = organization.PSRN;
        NCEO = organization.NCEO;
        NCEA = organization.NCEA;
        Account = organization.Account;
        Code = organization.Code;
        Nonresident = organization.Nonresident;
        ExternalId = organization.ExternalId;
    }
}