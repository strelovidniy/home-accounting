using HomeAccounting.Data.Entities;
using HomeAccounting.Models;
using HomeAccounting.Models.Views;

namespace HomeAccounting.UI.Domain.Services.Abstraction;

public interface IAuthService
{
    public Task<User?> GetCurrentUserAsync(
        CancellationToken cancellationToken = default
    );

    public Task<LoginView?> LoginAsync(
        LoginModel model,
        CancellationToken cancellationToken = default
    );
}