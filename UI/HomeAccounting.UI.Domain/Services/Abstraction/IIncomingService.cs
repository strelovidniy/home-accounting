using HomeAccounting.Models.Create;
using HomeAccounting.Models.Update;
using HomeAccounting.Models.Views;

namespace HomeAccounting.UI.Domain.Services.Abstraction;

public interface IIncomingService
{
    public Task CreateIncomingAsync(
        CreateIncomingModel model,
        CancellationToken cancellationToken = default
    );

    public Task UpdateIncomingAsync(
        UpdateIncomingModel model,
        CancellationToken cancellationToken = default
    );

    public Task DeleteIncomingAsync(
        Guid incomingId,
        CancellationToken cancellationToken = default
    );

    public Task<IncomingView?> GetIncomingAsync(
        Guid userId,
        Guid incomingId,
        CancellationToken cancellationToken = default
    );
}