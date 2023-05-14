using AutoMapper;
using HomeAccounting.Models.Create;

namespace HomeAccounting.Domain.Mapper.Converters.Spending;

internal class CreateSpendingModelToSpendingConverter : ITypeConverter<CreateSpendingModel, Data.Entities.Spending>
{
    public Data.Entities.Spending Convert(
        CreateSpendingModel createSpendingModel,
        Data.Entities.Spending spending,
        ResolutionContext context
    ) => new()
    {
        UserId = createSpendingModel.UserId,
        Amount = createSpendingModel.Amount,
        Description = createSpendingModel.Description
    };
}