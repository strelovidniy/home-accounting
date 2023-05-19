using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Http.HttpClientFactory;
using HomeAccounting.UI.Domain.Services.Abstraction;
using HomeAccounting.UI.Domain.Services.Realization;
using HomeAccounting.UI.Domain.Settings.Abstraction;
using HomeAccounting.UI.Domain.Settings.Realization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HomeAccounting.UI.Domain.DependencyInjection;

public static class UiConnectorDependencyInjectionExtension
{
    public static IServiceCollection RegisterUiConnectorLayer(
        this IServiceCollection services,
        IConfiguration configuration
    ) => services
        .AddServices()
        .AddSettings(configuration);

    private static IServiceCollection AddServices(
        this IServiceCollection services
    ) => services
        .AddScoped<IHttpClientFactory, HttpClientFactory>()
        .AddScoped<IHomeAccountingHttpClient, HomeAccountingHttpClient>()
        .AddTransient<IUserService, UserService>()
        .AddTransient<IAuthService, AuthService>()
        .AddTransient<ISpendingService, SpendingService>()
        .AddTransient<ICreditService, CreditService>()
        .AddTransient<IIncomingService, IncomingService>();

    private static IServiceCollection AddSettings(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var urlSettings = new UrlSettings();

        configuration
            .GetSection("UrlSettings")
            .Bind(urlSettings);

        services
            .AddTransient<IUrlSettings, UrlSettings>(_ => urlSettings);

        return services;
    }
}