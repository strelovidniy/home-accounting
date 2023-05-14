using AutoMapper;
using HomeAccounting.Data.Entities;
using HomeAccounting.Domain.Mapper.Converters.Incoming;
using HomeAccounting.Models.Create;
using HomeAccounting.Models.Views;

namespace HomeAccounting.Domain.Mapper.Profiles;

internal class IncomingMapperProfile : Profile
{
    public IncomingMapperProfile()
    {
        CreateMap<CreateIncomingModel, Incoming>().ConvertUsing(new CreateIncomingModelToIncomingConverter());

        CreateMap<Incoming, IncomingView>().ConvertUsing(new IncomingToIncomingViewConverter());
    }
}