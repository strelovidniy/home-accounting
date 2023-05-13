namespace HomeAccounting.Models;

public record ResetPasswordModel(
    string Email
) : IValidatableModel;