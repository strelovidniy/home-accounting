using HomeAccounting.Models.Create;
using HomeAccounting.Models.Update;
using HomeAccounting.Models.Views;
using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Services.Abstraction;

namespace HomeAccounting.UI.Domain.Services.Realization;

internal class IncomingService : IIncomingService
{
    private readonly IHomeAccountingHttpClient _httpClient;

    public IncomingService(
        IHomeAccountingHttpClient httpClient
    ) => _httpClient = httpClient;

    public Task CreateIncomingAsync(
        CreateIncomingModel model,
        CancellationToken cancellationToken = default
    ) => _httpClient
        .PostAsync(
            "api/v1/incomings",
            _httpClient.CreateJsonContent(model),
            cancellationToken
        );

    public Task UpdateIncomingAsync(
        UpdateIncomingModel model,
        CancellationToken cancellationToken = default
    ) => _httpClient
        .PutAsync(
            "api/v1/incomings",
            _httpClient.CreateJsonContent(model),
            cancellationToken
        );

    public Task DeleteIncomingAsync(
        Guid incomingId,
        CancellationToken cancellationToken = default
    ) => _httpClient
        .DeleteAsync(
            $"api/v1/incomings?incomingId={incomingId}",
            cancellationToken
        );

    public Task<IncomingView?> GetIncomingAsync(
        Guid userId,
        Guid incomingId,
        CancellationToken cancellationToken = default
    ) => _httpClient
        .GetAsync<IncomingView>(
            $"api/v1/incomings?userId={userId}&incomingId={incomingId}",
            cancellationToken
        );

    public Task<decimal> GetAverageIncomingAsync(
        CancellationToken cancellationToken = default
    ) => _httpClient
        .GetAsync<decimal>(
            "api/v1/incomings/average",
            cancellationToken
        );
}