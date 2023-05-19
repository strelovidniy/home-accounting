using AutoMapper;
using HomeAccounting.Models.Views;

namespace HomeAccounting.Domain.Mapper.Converters.Deposit;

internal class DepositToDepositViewConverter : ITypeConverter<Data.Entities.Deposit, DepositView>
{
    public DepositView Convert(
        Data.Entities.Deposit deposit,
        DepositView DepositView,
        ResolutionContext context
    ) => new()
    {
        User = context.Mapper.Map<UserView>(deposit.User),
        Id = deposit.Id,
        Amount = deposit.Amount,
        Description = deposit.Description,
        DepositDate = deposit.CreatedAt,
        DepositUpdatedAt = deposit.UpdatedAt
    };
}