using HomeAccounting.Models.Create;
using HomeAccounting.Models.Update;
using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Services.Abstraction;

namespace HomeAccounting.UI.Domain.Services.Realization;

internal class CreditService : ICreditService
{
    private readonly IHomeAccountingHttpClient _httpClient;

    public CreditService(
        IHomeAccountingHttpClient httpClient
    ) => _httpClient = httpClient;

    public Task CreateCreditAsync(CreateCreditModel model, CancellationToken cancellationToken = default)
    => _httpClient
        .PostAsync(
            "api/v1/credits",
            _httpClient.CreateJsonContent(model),
            cancellationToken
        );

    public Task UpdateCreditAsync(UpdateCreditModel model, CancellationToken cancellationToken = default)
    =>_httpClient
        .PutAsync(
            "api/v1/credits",
            _httpClient.CreateJsonContent(model),
            cancellationToken
        );

    public Task DeleteCreditAsync(Guid creditId, CancellationToken cancellationToken = default)
        => _httpClient
            .DeleteAsync(
                $"api/v1/credits?creditId={creditId}",
                cancellationToken
            );
}