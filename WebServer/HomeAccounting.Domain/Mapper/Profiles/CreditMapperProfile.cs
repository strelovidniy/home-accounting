using AutoMapper;
using HomeAccounting.Data.Entities;
using HomeAccounting.Domain.Mapper.Converters.Credit;
using HomeAccounting.Domain.Mapper.Converters.Incoming;
using HomeAccounting.Models.Create;
using HomeAccounting.Models.Views;

namespace HomeAccounting.Domain.Mapper.Profiles;

internal class CreditMapperProfile : Profile
{
    public CreditMapperProfile()
    {
        CreateMap<CreateCreditModel, Credit>().ConvertUsing(new CreateCreateModelToCreditConverter());

        CreateMap<Credit, CreditView>().ConvertUsing(new CreditToCreditViewConverter());
    }
}