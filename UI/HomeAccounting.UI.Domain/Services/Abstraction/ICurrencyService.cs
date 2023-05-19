using HomeAccounting.Models.Views;

namespace HomeAccounting.UI.Domain.Services.Abstraction;

public interface ICurrencyService
{
    public Task<IEnumerable<CurrencyView>?> GetCurrenciesAsync(
        CancellationToken cancellationToken = default
    );
}