using Microsoft.Extensions.DependencyInjection;

namespace HomeAccounting.UI.Shared.DependencyInjection;

public static class DependencyInjectionExtension
{
    public static IServiceCollection RegisterUiShared(
        this IServiceCollection services
    ) => services;
}