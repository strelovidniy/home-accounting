using AutoMapper;
using FluentValidation;
using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Domain.Services.Realization;
using HomeAccounting.Domain.Settings.Abstraction;
using HomeAccounting.Domain.Settings.Realization;
using HomeAccounting.Domain.Validators;
using HomeAccounting.Models;
using HomeAccounting.Models.Change;
using HomeAccounting.Models.Create;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HomeAccounting.Domain.DependencyInjection;

public static class DomainDependencyInjectionExtension
{
    public static IServiceCollection RegisterDomainLayer(
        this IServiceCollection services,
        IConfiguration configuration
    ) => services
        .AddServices()
        .AddValidators()
        .AddSettings(configuration);

    private static IServiceCollection AddServices(
        this IServiceCollection services
    ) => services
        .AddHttpContextAccessor()
        .AddTransient<IEmailService, EmailService>()
        .AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>()
        .AddTransient<IValidationService, ValidationService>()
        .AddTransient<IUserService, UserService>()
        .AddTransient<IAuthService, AuthService>();

    private static IServiceCollection AddValidators(
        this IServiceCollection services
    ) => services
        .AddTransient<IValidator<LoginModel>, LoginModelValidator>()
        .AddTransient<IValidator<ChangePasswordModel>, ChangePasswordModelValidator>()
        .AddTransient<IValidator<CreateUserModel>, CreateUserModelValidator>()
        .AddTransient<IValidator<ResetPasswordModel>, ResetPasswordModelValidator>()
        .AddTransient<IValidator<SetNewPasswordModel>, SetNewPasswordModelValidator>();

    private static IServiceCollection AddMapper(
        this IServiceCollection services
    ) => services
        .AddAutoMapper(config => config.AddProfiles(new List<Profile>()));

    private static IServiceCollection AddSettings(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var jwtSettings = new JwtSettings();
        var emailSettings = new EmailSettings();
        var urlSettings = new UrlSettings();

        configuration
            .GetSection("AccessTokenSettings")
            .Bind(jwtSettings);

        configuration
            .GetSection("EmailSettings")
            .Bind(emailSettings);

        configuration
            .GetSection("UrlSettings")
            .Bind(urlSettings);

        services
            .AddTransient<IJwtSettings, JwtSettings>(_ => jwtSettings)
            .AddTransient<IEmailSettings, EmailSettings>(_ => emailSettings)
            .AddTransient<IUrlSettings, UrlSettings>(_ => urlSettings)
            .AddTransient<IJwtSettings, JwtSettings>(_ => jwtSettings);

        return services;
    }
}