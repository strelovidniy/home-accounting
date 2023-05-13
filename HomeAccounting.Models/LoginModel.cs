namespace HomeAccounting.Models;

public class LoginModel : IValidatableModel
{
    public string Email { get; set; }

    public string Password { get; set; }
}