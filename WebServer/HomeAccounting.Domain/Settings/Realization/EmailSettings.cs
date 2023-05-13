using HomeAccounting.Domain.Settings.Abstraction;

namespace HomeAccounting.Domain.Settings.Realization;

internal class EmailSettings : IEmailSettings
{
    public string Password { get; set; } = null!;

    public string FromEmail { get; set; } = null!;

    public string FromDisplayName { get; set; } = null!;
}