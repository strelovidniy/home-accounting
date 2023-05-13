using FluentValidation;
using HomeAccounting.Data.Enums;
using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Domain.Validators.Extensions;
using HomeAccounting.Models;

namespace HomeAccounting.Domain.Validators;

internal class ResetPasswordModelValidator : AbstractValidator<ResetPasswordModel>
{
    public ResetPasswordModelValidator(IValidationService validationService)
    {
        RuleFor(resetPasswordModel => resetPasswordModel.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithStatusCode(StatusCode.EmailRequired)
            .EmailAddress()
            .WithStatusCode(StatusCode.InvalidEmailFormat)
            .MustAsync(validationService.IsUserExistsAsync)
            .WithStatusCode(StatusCode.UserNotFound);
    }
}