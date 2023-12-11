using Microsoft.AspNetCore.Identity;

namespace DirectumCommunity.Models;

public class DirectumUser : IdentityUser
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
}