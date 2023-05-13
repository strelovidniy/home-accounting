using HomeAccounting.Data.Enums.RichEnums;
using HomeAccounting.Domain.Models.ViewModels;

namespace HomeAccounting.Domain.Services.Abstraction;

public interface IRazorViewToStringRenderer
{
    public Task<string> RenderViewToStringAsync<TModel>(
        EmailViewLocation viewName,
        TModel model
    ) where TModel : class, IEmailViewModel;
}