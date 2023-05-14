using AutoMapper;
using HomeAccounting.Data.Entities;
using HomeAccounting.Domain.Mapper.Converters.Spending;
using HomeAccounting.Models.Create;
using HomeAccounting.Models.Views;

namespace HomeAccounting.Domain.Mapper.Profiles;

internal class SpendingMapperProfile : Profile
{
    public SpendingMapperProfile()
    {
        CreateMap<CreateSpendingModel, Spending>().ConvertUsing(new CreateSpendingModelToSpendingConverter());

        CreateMap<Spending, SpendingView>().ConvertUsing(new SpendingToSpendingViewConverter());
    }
}