using HomeAccounting.Models.Create;
using HomeAccounting.Models.Update;
using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Services.Abstraction;

namespace HomeAccounting.UI.Domain.Services.Realization;

internal class DepositService : IDepositService
{
    private readonly IHomeAccountingHttpClient _httpClient;

    public DepositService(
        IHomeAccountingHttpClient httpClient
    ) => _httpClient = httpClient;

    public Task CreateDepositAsync(CreateDepositModel model, CancellationToken cancellationToken = default)
        => _httpClient
        .PostAsync(
            "api/v1/deposits",
            _httpClient.CreateJsonContent(model),
            cancellationToken
        );

    public Task UpdateDepositAsync(UpdateDepositModel model, CancellationToken cancellationToken = default)
        => _httpClient
            .PutAsync(
                "api/v1/deposits",
                _httpClient.CreateJsonContent(model),
                cancellationToken
            );

    public Task DeleteDepositAsync(Guid depositId, CancellationToken cancellationToken = default)
        => _httpClient
            .DeleteAsync(
                $"api/v1/deposits?depositId={depositId}",
                cancellationToken
            );
}