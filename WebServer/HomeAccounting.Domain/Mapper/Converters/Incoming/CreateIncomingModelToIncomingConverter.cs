using AutoMapper;
using HomeAccounting.Models.Create;

namespace HomeAccounting.Domain.Mapper.Converters.Incoming;

internal class CreateIncomingModelToIncomingConverter : ITypeConverter<CreateIncomingModel, Data.Entities.Incoming>
{
    public Data.Entities.Incoming Convert(
        CreateIncomingModel createIncomingModel,
        Data.Entities.Incoming incoming,
        ResolutionContext context
    ) => new()
    {
        Amount = createIncomingModel.Amount,
        Description = createIncomingModel.Description,
        UserId = createIncomingModel.UserId
    };
}