using HomeAccounting.Data.Entities;
using HomeAccounting.Models;
using HomeAccounting.Models.Change;
using HomeAccounting.Models.Create;
using HomeAccounting.Models.Views;
using Microsoft.AspNetCore.Http;

namespace HomeAccounting.Domain.Services.Abstraction;

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

    public Task<UserView?> GetUserAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    public Task ChangePasswordAsync(
        ChangePasswordModel changePasswordModel,
        CancellationToken cancellationToken = default
    );

    Task<User> GetPureUserAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    public Task ChangeAvatarAsync(
        IFormFile avatar,
        CancellationToken cancellationToken = default
    );
}