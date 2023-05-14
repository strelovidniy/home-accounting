using HomeAccounting.Models;
using HomeAccounting.Models.Views;

namespace HomeAccounting.Domain.Services.Abstraction;

public interface IAuthService
{
    public Task<UserView?> GetCurrentUserAsync(
        CancellationToken cancellationToken = default
    );

    public Task<LoginView?> LoginAsync(
        LoginModel model,
        CancellationToken cancellationToken = default
    );
}