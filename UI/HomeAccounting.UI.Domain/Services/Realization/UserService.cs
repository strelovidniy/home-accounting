using HomeAccounting.Models;
using HomeAccounting.Models.Change;
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

    public Task ChangeAvatarAsync(
        byte[] bytes,
        CancellationToken cancellationToken = default
    ) => _httpClient
        .PostAsync(
            "api/v1/users/change-avatar",
            new MultipartFormDataContent
            {
                new ByteArrayContent(bytes)
            },
            cancellationToken
        );

    public Task<string?> GetMonobankTokenAsync(
        CancellationToken cancellationToken = default
    ) => _httpClient
        .GetAsync<string>(
            "api/v1/users/monobank-token",
            cancellationToken
        );

    public Task SetMonobankTokenAsync(
        SetMonobankTokenModel monobankTokenModel,
        CancellationToken cancellationToken = default
    ) => _httpClient
        .PutAsync(
            "api/v1/users/monobank-token",
            _httpClient.CreateJsonContent(monobankTokenModel),
            cancellationToken
        );

    public Task ChangePasswordAsync(
        ChangePasswordModel changePasswordModel,
        CancellationToken cancellationToken = default
    ) => _httpClient
        .PutAsync(
            "api/v1/users/change-password",
            _httpClient.CreateJsonContent(changePasswordModel),
            cancellationToken
        );
}