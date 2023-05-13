namespace HomeAccounting.Domain.Settings.Abstraction;

public interface IAuthSettings
{
    IEnumerable<string> AllowedOrigins { get; set; }
}