using System.Text.Json;
using AutoMapper;
using HomeAccounting.Domain.Models;
using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Models.Views;

namespace HomeAccounting.Domain.Services.Realization;

internal class CurrencyService : ICurrencyService
{
    private readonly IMapper _mapper;
    private IEnumerable<CurrencyView> _cachedCurrencies = new List<CurrencyView>();

    public CurrencyService(
        IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<CurrencyView>> GetCurrenciesAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            _cachedCurrencies = _mapper
                .Map<IEnumerable<CurrencyView>>(
                    JsonSerializer
                        .Deserialize<IEnumerable<CurrencyModel>>(
                            await (await new HttpClient().GetAsync(
                                    "https://api.monobank.ua/bank/currency",
                                    cancellationToken
                                ))
                                .Content
                                .ReadAsStringAsync(cancellationToken),
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            }
                        )
                );
        }
        catch
        {
            // ignored
        }

        return _cachedCurrencies;
    }
    

}