using AutoMapper;
using HomeAccounting.Models.Views;

namespace HomeAccounting.Domain.Mapper.Converters.Incoming;

internal class IncomingToIncomingViewConverter : ITypeConverter<Data.Entities.Incoming, IncomingView>
{
    public IncomingView Convert(
        Data.Entities.Incoming incoming,
        IncomingView incomingView,
        ResolutionContext context
    ) => new()
    {
        User = context.Mapper.Map<UserView>(incoming.User),
        Id = incoming.Id,
        Amount = incoming.Amount,
        Description = incoming.Description,
        IncomingDate = incoming.CreatedAt,
        IncomingUpdatedAt = incoming.UpdatedAt
    };
}