using HomeAccounting.Models.Create;
using HomeAccounting.Models.Update;
using HomeAccounting.Models.Views;
using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Services.Abstraction;

namespace HomeAccounting.UI.Domain.Services.Realization;

internal class SpendingService : ISpendingService
{
    private readonly IHomeAccountingHttpClient _httpClient;

    public SpendingService(
        IHomeAccountingHttpClient httpClient
    ) => _httpClient = httpClient;

    public Task CreateSpendingAsync(
        CreateSpendingModel model,
        CancellationToken cancellationToken = default
    ) => _httpClient
        .PostAsync(
            "api/v1/spendings",
            _httpClient.CreateJsonContent(model),
            cancellationToken
        );

    public Task UpdateSpendingAsync(
        UpdateSpendingModel model,
        CancellationToken cancellationToken = default
    ) => _httpClient
        .PutAsync(
            "api/v1/spendings",
            _httpClient.CreateJsonContent(model),
            cancellationToken
        );

    public Task DeleteSpendingAsync(
        Guid spendingId,
        CancellationToken cancellationToken = default
    ) => _httpClient
        .DeleteAsync(
            $"api/v1/spendings?spendingId={spendingId}",
            cancellationToken
        );

    public Task<SpendingView?> GetSpendingAsync(
        Guid userId,
        Guid spendingId,
        CancellationToken cancellationToken = default
    ) => _httpClient
        .GetAsync<SpendingView>(
            $"api/v1/spendings?userId={userId}&spendingId={spendingId}",
            cancellationToken
        );
}