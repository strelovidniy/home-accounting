using AutoMapper;
using HomeAccounting.Models.Create;

namespace HomeAccounting.Domain.Mapper.Converters.Deposit;

internal class CreateDepositModelToDepositConverter : ITypeConverter<CreateDepositModel, Data.Entities.Deposit>
{
    public Data.Entities.Deposit Convert(
        CreateDepositModel createDepositModel,
        Data.Entities.Deposit deposit,
        ResolutionContext context
    ) => new()
    {
        Amount = createDepositModel.Amount,
        Description = createDepositModel.Description,
        RateOfInterest = createDepositModel.RateOfInterest,
        NumberOfYears = createDepositModel.NumberOfYears,
        UserId = createDepositModel.UserId
    };
}