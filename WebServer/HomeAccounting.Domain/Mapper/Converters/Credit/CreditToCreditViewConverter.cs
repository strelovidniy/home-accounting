using AutoMapper;
using HomeAccounting.Models.Views;

namespace HomeAccounting.Domain.Mapper.Converters.Credit;

internal class CreditToCreditViewConverter : ITypeConverter<Data.Entities.Credit, CreditView>
{
    public CreditView Convert(
        Data.Entities.Credit credit,
        CreditView creditView,
        ResolutionContext context
    ) => new()
    {
        User = context.Mapper.Map<UserView>(credit.User),
        Id = credit.Id,
        Amount = credit.Amount,
        Description = credit.Description,
        CreditDate = credit.CreatedAt,
        CreditUpdatedAt = credit.UpdatedAt
    };
}