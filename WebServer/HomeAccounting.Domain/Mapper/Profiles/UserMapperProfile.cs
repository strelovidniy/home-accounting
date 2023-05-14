using AutoMapper;
using HomeAccounting.Data.Entities;
using HomeAccounting.Domain.Mapper.Converters.User;
using HomeAccounting.Models.Create;
using HomeAccounting.Models.Views;

namespace HomeAccounting.Domain.Mapper.Profiles;

internal class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<CreateUserModel, User>().ConvertUsing(new CreateUserModelToUserConverter());

        CreateMap<User, UserView>().ConvertUsing(new UserToUserViewConverter());
    }
}