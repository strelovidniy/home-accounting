using AutoMapper;
using HomeAccounting.Models.Views;

namespace HomeAccounting.Domain.Mapper.Converters.Spending;

internal class SpendingToSpendingViewConverter : ITypeConverter<Data.Entities.Spending, SpendingView>
{
    public SpendingView Convert(
        Data.Entities.Spending spending,
        SpendingView spendingView,
        ResolutionContext context
    ) => new()
    {
        User = context.Mapper.Map<UserView>(spending.User),
        Id = spending.Id,
        Amount = spending.Amount,
        Description = spending.Description,
        SpendingDate = spending.CreatedAt,
        SpendingUpdatedAt = spending.UpdatedAt
    };
}