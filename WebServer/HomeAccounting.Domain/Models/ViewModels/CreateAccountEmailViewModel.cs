namespace HomeAccounting.Domain.Models.ViewModels;

public record CreateAccountEmailViewModel(
    string Url
) : IEmailViewModel;