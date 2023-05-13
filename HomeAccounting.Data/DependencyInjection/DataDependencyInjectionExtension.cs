using EntityFrameworkCore.RepositoryInfrastructure.DependencyInjection;
using HomeAccounting.Data.Context;
using HomeAccounting.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HomeAccounting.Data.DependencyInjection;

public static class DataDependencyInjectionExtension
{
    public static IServiceCollection RegisterDataLayer(
        this IServiceCollection services,
        IConfiguration configuration
    ) => services
        .AddDbContext(configuration)
        .AddRepositories();

    private static IServiceCollection AddDbContext(
        this IServiceCollection services,
        IConfiguration configuration
    ) => services.AddDbContext<HomeAccountingContext>(options =>
    {
        options.UseSqlServer(configuration.GetConnectionString("SqlServer"))
            .EnableSensitiveDataLogging();
    });

    private static IServiceCollection AddRepositories(
        this IServiceCollection services
    ) => services
        .CreateRepositoryBuilderWithContext<HomeAccountingContext>()
        .AddRepository<User>()
        .Build();
}