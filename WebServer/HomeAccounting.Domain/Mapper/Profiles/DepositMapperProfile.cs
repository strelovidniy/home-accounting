using AutoMapper;
using HomeAccounting.Data.Entities;
using HomeAccounting.Domain.Mapper.Converters.Deposit;
using HomeAccounting.Models.Create;
using HomeAccounting.Models.Views;

namespace HomeAccounting.Domain.Mapper.Profiles;

internal class DepositMapperProfile : Profile
{
    public DepositMapperProfile()
    {
        CreateMap<CreateDepositModel, Deposit>().ConvertUsing(new CreateDepositModelToDepositConverter());

        CreateMap<Deposit, DepositView>().ConvertUsing(new DepositToDepositViewConverter());
    }
}