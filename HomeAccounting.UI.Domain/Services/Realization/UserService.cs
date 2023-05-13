using HomeAccounting.Models;
using HomeAccounting.Models.Create;
using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Services.Abstraction;

namespace HomeAccounting.UI.Domain.Services.Realization;

internal class UserService : IUserService
{
    private readonly IHomeAccountingHttpClient _httpClient;

    public UserService(
        IHomeAccountingHttpClient httpClient
    ) => _httpClient = httpClient;

    public Task ResetPasswordAsync(
        ResetPasswordModel resetPasswordModel,
        CancellationToken cancellationToken = default
    ) => _httpClient
        .PostAsync(
            "api/v1/users/reset-password",
            _httpClient.CreateJsonContent(resetPasswordModel),
            cancellationToken
        );

    public Task SetNewPasswordAsync(
        SetNewPasswordModel setNewPasswordModel,
        CancellationToken cancellationToken = default
    ) => _httpClient
        .PostAsync(
            "api/v1/users/set-new-password",
            _httpClient.CreateJsonContent(setNewPasswordModel),
            cancellationToken
        );

    public Task CreateUserAsync(
        CreateUserModel createUserModel,
        CancellationToken cancellationToken = default
    ) => _httpClient
        .PostAsync(
            "api/v1/users/sign-up",
            _httpClient.CreateJsonContent(createUserModel),
            cancellationToken
        );
}