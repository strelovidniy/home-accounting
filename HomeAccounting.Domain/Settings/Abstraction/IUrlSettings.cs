namespace HomeAccounting.Domain.Settings.Abstraction;

public interface IUrlSettings
{
    public string ResetPasswordUrl { get; set; }

    public string RegisterUrl { get; set; }

    public string WebApiUrl { get; set; }

    public string P3TwitterUrl { get; set; }

    public string P3FacebookUrl { get; set; }

    public string P3InstagramUrl { get; set; }
}