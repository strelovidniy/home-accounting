namespace HomeAccounting.Models;

public class AuthToken
{
    public string? Token { get; set; }

    public string? ExpireAt { get; set; }

    public string? RefreshToken { get; set; }
}