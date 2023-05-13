﻿using System.Text.Json.Serialization;
using HomeAccounting.Data.DependencyInjection;
using HomeAccounting.Data.Entities;
using HomeAccounting.Domain.DependencyInjection;
using HomeAccounting.Domain.Extensions;
using HomeAccounting.Domain.Helpers;
using HomeAccounting.Domain.Json.Converters;
using HomeAccounting.Domain.Middleware;
using HomeAccounting.Domain.Settings.Realization;
using HomeAccounting.Server.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace HomeAccounting.Server.DependencyInjection;

public static class DependencyInjectionExtension
{
    private const string MainPolicyName = "_main_policy_";

    public static IServiceCollection RegisterApplication(
        this IServiceCollection services,
        IConfiguration configuration
    ) => services
        .AddMvc()
        .Services
        .RegisterDataLayer(configuration)
        .RegisterDomainLayer(configuration)
        .RegisterWebApi(configuration);

    private static IServiceCollection RegisterWebApi(
        this IServiceCollection services,
        IConfiguration configuration
    ) => services
        .AddHttpClient()
        .RegisterCors(configuration)
        .RegisterControllers(configuration)
        .RegisterAuth(configuration)
        .RegisterSwagger();

    private static IServiceCollection RegisterCors(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var authSettings = new AuthSettings();

        configuration.GetSection("AuthSettings").Bind(authSettings);

        return services.AddCors(options => options.AddPolicy(
            MainPolicyName,
            policyBuilder => policyBuilder
                .WithOrigins(authSettings.AllowedOrigins.ToNullSafeArray())
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()));
    }

    private static IServiceCollection RegisterAuth(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var jwtSettings = new JwtSettings();

        configuration.GetSection("AccessTokenSettings").Bind(jwtSettings);

        return services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => x.TokenValidationParameters = JwtHelper.GetTokenValidationParameters(jwtSettings))
            .Services
            .AddAuthorization();
    }

    private static IServiceCollection RegisterSwagger(
        this IServiceCollection services
    ) => services.AddSwaggerGen(swaggerGenOptions =>
    {
        swaggerGenOptions.SwaggerDoc(
            "v1",
            new OpenApiInfo
            {
                Title = "Home Accounting API",
                Version = "v1"
            }
        );

        swaggerGenOptions.AddSecurityDefinition(
            "Bearer",
            new OpenApiSecurityScheme
            {
                Description = "Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            }
        );

        swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });

        swaggerGenOptions.CustomSchemaIds(type => type.FullName);
    });

    public static IMvcBuilder AddCustomBadRequest(this IMvcBuilder services)
    {
        services.ConfigureApiBehaviorOptions(options =>
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var allErrors = actionContext.ModelState.Values.SelectMany(v => v.Errors);

                return new BadRequestObjectResult(new
                {
                    StatusCode = 400,
                    Message = string.Join(" - ", allErrors.Select(e => e.ErrorMessage))
                });
            });

        return services;
    }


    private static IServiceCollection RegisterControllers(
        this IServiceCollection services,
        IConfiguration configuration
    ) =>
        services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ApiExceptionFilter>();
                options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();

                options.CacheProfiles.Add(
                    ResponseCacheProfile.StaticDataCacheProfile,
                    new CacheProfile
                    {
                        Duration = int.Parse(
                            configuration.GetSection("ResponseCaching")["StaticDataCacheDurationSeconds"]!
                        )
                    }
                );
            })
            .AddCustomBadRequest()
            .AddOData(options =>
            {
                options.AddRouteComponents("api/odata", GetEdmModel());
                options.EnableQueryFeatures(500);
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;

                options.SerializerSettings.Converters.Add(
                    new JsonDateTimeConverter()
                );

                options.SerializerSettings.Converters.Add(
                    new StringEnumConverter(
                        new CamelCaseNamingStrategy(),
                        false
                    )
                );
            })
            .Services
            .AddSignalR()
            .AddJsonProtocol(options =>
                options.PayloadSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault)
            .Services
            .AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

    public static WebApplication UseCorsConfiguration(this WebApplication app)
    {
        app.UseCors(MainPolicyName);

        return app;
    }

    public static WebApplication UseAuthConfiguration(this WebApplication app)
    {
        app
            .UseAuthentication()
            .UseAuthorization();

        return app;
    }

    public static IApplicationBuilder UseApplication(this WebApplication app)
    {
        app.UseCorsConfiguration();

        if (app.Environment.IsProduction())
        {
            app.UseHsts();
        }
        else
        {
            app.UseWebAssemblyDebugging();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.UseRouting();

        app.MapRazorPages();
        app.UseAuthConfiguration();

        app.MapControllers();

        app.MapFallbackToFile("index.html");

        return app;
    }

    private static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();

        builder.EntitySet<User>("users").EntityType.HasKey(e => e.Id);

        return builder.GetEdmModel();
    }
}