using AutoMapper;
using HomeAccounting.Models.Views;

namespace HomeAccounting.Domain.Mapper.Converters.User;

internal class UserToUserViewConverter : ITypeConverter<Data.Entities.User, UserView>
{
    public UserView Convert(
        Data.Entities.User user,
        UserView userView,
        ResolutionContext context
    ) => new()
    {
        Id = user.Id,
        Email = user.Email,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Status = user.Status,
        ImageDataUrl = user.ImageDataUrl
    };
}