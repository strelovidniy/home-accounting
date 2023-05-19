using AutoMapper;
using HomeAccounting.Models.Create;

namespace HomeAccounting.Domain.Mapper.Converters.Credit;

internal class CreateCreateModelToCreditConverter : ITypeConverter<CreateCreditModel, Data.Entities.Credit>
{
    public Data.Entities.Credit Convert(
        CreateCreditModel createCreditModel,
        Data.Entities.Credit credit,
        ResolutionContext context
    ) => new()
    {
        Amount = createCreditModel.Amount,
        Description = createCreditModel.Description,
        UserId = createCreditModel.UserId
    };
}