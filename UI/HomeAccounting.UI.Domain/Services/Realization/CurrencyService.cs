using HomeAccounting.Models.Views;
using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Services.Abstraction;

namespace HomeAccounting.UI.Domain.Services.Realization;

internal class CurrencyService : ICurrencyService
{
    private readonly IHomeAccountingHttpClient _httpClient;

    public CurrencyService(
        IHomeAccountingHttpClient httpClient
    ) => _httpClient = httpClient;

    public Task<IEnumerable<CurrencyView>?> GetCurrenciesAsync(
        CancellationToken cancellationToken = default
    ) => _httpClient
        .GetAsync<IEnumerable<CurrencyView>>(
            "api/v1/currency",
            cancellationToken
        );
}