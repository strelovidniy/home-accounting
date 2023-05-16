using HomeAccounting.UI.Domain.Settings.Abstraction;

namespace HomeAccounting.UI.Domain.Settings.Realization;

internal class UrlSettings : IUrlSettings
{
    public string WebApiUrl { get; set; } = null!;
}