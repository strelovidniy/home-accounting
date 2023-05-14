using HomeAccounting.Models;
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
}