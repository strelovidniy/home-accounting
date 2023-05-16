using HomeAccounting.Models;
using HomeAccounting.Models.Change;
using HomeAccounting.Models.Create;

namespace HomeAccounting.UI.Domain.Services.Abstraction;

public interface IUserService
{
    public Task ResetPasswordAsync(
        ResetPasswordModel resetPasswordModel,
        CancellationToken cancellationToken = default
    );

    public Task SetNewPasswordAsync(
        SetNewPasswordModel setNewPasswordModel,
        CancellationToken cancellationToken = default
    );

    public Task CreateUserAsync(
        CreateUserModel createUserModel,
        CancellationToken cancellationToken = default
    );

    public Task ChangeAvatarAsync(
        byte[] bytes,
        CancellationToken cancellationToken = default
    );

    public Task<string?> GetMonobankTokenAsync(
        CancellationToken cancellationToken = default
    );

    public Task SetMonobankTokenAsync(
        SetMonobankTokenModel monobankToken,
        CancellationToken cancellationToken = default
    );

    public Task ChangePasswordAsync(
        ChangePasswordModel changePasswordModel,
        CancellationToken cancellationToken = default
    );
}