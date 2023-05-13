using HomeAccounting.Server.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.RegisterApplication(builder.Configuration);

var app = builder.Build();

app.UseApplication();

app.Run();