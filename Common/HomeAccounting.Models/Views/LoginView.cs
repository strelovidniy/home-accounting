using HomeAccounting.Data.Entities;

namespace HomeAccounting.Models.Views;

public class LoginView
{
    public User User { get; set; }

    public AuthToken Token { get; set; }
}