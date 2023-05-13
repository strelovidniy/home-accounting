using HomeAccounting.UI.Domain.Settings.Abstraction;

namespace HomeAccounting.UI.Domain.Settings.Realization;

internal class UrlSettings : IUrlSettings
{
    public string ResetPasswordUrl { get; set; } = null!;

    public string RegisterUrl { get; set; } = null!;

    public string WebApiUrl { get; set; } = null!;

    public string P3TwitterUrl { get; set; } = null!;

    public string P3FacebookUrl { get; set; } = null!;

    public string P3InstagramUrl { get; set; } = null!;
}