using HomeAccounting.Models.Create;
using HomeAccounting.Models.Update;

namespace HomeAccounting.Domain.Services.Abstraction;

public interface ICreditService
{
    public Task CreateCreditAsync(
        CreateCreditModel model,
        CancellationToken cancellationToken = default
    );

    public Task UpdateCreditAsync(
        UpdateCreditModel model,
        CancellationToken cancellationToken = default
    );

    public Task DeleteCreditAsync(
        Guid creditId,
        CancellationToken cancellationToken = default
    );
}