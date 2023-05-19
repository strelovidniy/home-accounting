using AutoMapper;
using HomeAccounting.Domain.Mapper.Converters.Currency;
using HomeAccounting.Domain.Models;
using HomeAccounting.Models.Views;

namespace HomeAccounting.Domain.Mapper.Profiles;

internal class CurrencyMapperProfile : Profile
{
    public CurrencyMapperProfile()
    {
        CreateMap<CurrencyModel, CurrencyView>().ConvertUsing(new CurrencyModelToCurrencyViewConverter());
    }
}