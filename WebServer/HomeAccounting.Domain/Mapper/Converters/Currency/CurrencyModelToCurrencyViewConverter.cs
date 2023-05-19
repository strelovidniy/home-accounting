using AutoMapper;
using HomeAccounting.Domain.Models;
using HomeAccounting.Models.Views;

namespace HomeAccounting.Domain.Mapper.Converters.Currency;

internal class CurrencyModelToCurrencyViewConverter : ITypeConverter<CurrencyModel, CurrencyView>
{
    public CurrencyView Convert(
        CurrencyModel currencyModel,
        CurrencyView currencyView,
        ResolutionContext context
    ) => new(
        currencyModel.CurrencyCodeA.ToString(),
        currencyModel.CurrencyCodeB.ToString(),
        currencyModel.RateBuy,
        currencyModel.RateSell
    );
}