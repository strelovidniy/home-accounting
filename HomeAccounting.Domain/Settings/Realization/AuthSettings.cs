using HomeAccounting.Domain.Settings.Abstraction;

namespace HomeAccounting.Domain.Settings.Realization;

public class AuthSettings : IAuthSettings
{
    public IEnumerable<string> AllowedOrigins { get; set; } = null!;
}