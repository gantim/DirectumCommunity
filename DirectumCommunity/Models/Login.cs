namespace DirectumCommunity.Models;

public class Login
{
    public int Id { get; set; }
    public bool? NeedChangePassword { get; set; }
    public string? LoginName { get; set; }
    public string? TypeAuthentication { get; set; }
    public string? Status { get; set; }
    private DateTimeOffset? passwordLastChangeDate;
    public DateTimeOffset? PasswordLastChangeDate
    {
        get => passwordLastChangeDate;
        set
        {
            if (value.HasValue)
                passwordLastChangeDate = value.Value.UtcDateTime;
            else
                passwordLastChangeDate = null;
        }
    }
    private DateTimeOffset? lockoutEndDate;
    public DateTimeOffset? LockoutEndDate
    {
        get => lockoutEndDate;
        set
        {
            if (value.HasValue)
                lockoutEndDate = value.Value.UtcDateTime;
            else
                lockoutEndDate = null;
        }
    }
}