using HomeAccounting.Data.Entities;
using HomeAccounting.Models;
using HomeAccounting.Models.Views;
using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Services.Abstraction;

namespace HomeAccounting.UI.Domain.Services.Realization;

internal class AuthService : IAuthService
{
    private readonly IHomeAccountingHttpClient _httpClient;

    public AuthService(
        IHomeAccountingHttpClient httpClient
    ) => _httpClient = httpClient;

    public Task<LoginView?> LoginAsync(
        LoginModel model,
        CancellationToken cancellationToken = default
    ) => _httpClient
        .PostAsync<LoginView>(
            "api/v1/auth/login",
            _httpClient.CreateJsonContent(model),
            cancellationToken
        );

    public Task<User?> GetCurrentUserAsync(
        CancellationToken cancellationToken = default
    ) => _httpClient
        .GetAsync<User>(
            "api/v1/auth/me",
            cancellationToken
        );
}