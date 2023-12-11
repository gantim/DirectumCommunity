using System.ComponentModel.DataAnnotations;

namespace DirectumCommunity.Models;

public class LoginViewModel
{
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
}