using HomeAccounting.Models.Create;
using HomeAccounting.Models.Update;
using HomeAccounting.Models.Views;

namespace HomeAccounting.UI.Domain.Services.Abstraction;

public interface ISpendingService
{
    public Task CreateSpendingAsync(
        CreateSpendingModel model,
        CancellationToken cancellationToken = default
    );

    public Task UpdateSpendingAsync(
        UpdateSpendingModel model,
        CancellationToken cancellationToken = default
    );

    public Task DeleteSpendingAsync(
        Guid spendingId,
        CancellationToken cancellationToken = default
    );

    public Task<SpendingView?> GetSpendingAsync(
        Guid userId,
        Guid spendingId,
        CancellationToken cancellationToken = default
    );

    public Task<decimal> GetAverageSpendingAsync(
        CancellationToken cancellationToken = default
    );
}