namespace HomeAccounting.Domain.Models.ViewModels;

public record ResetPasswordEmailViewModel(
    string Url
) : IEmailViewModel;