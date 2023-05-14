using FluentValidation;
using HomeAccounting.Data.Enums;
using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Domain.Validators.Extensions;
using HomeAccounting.Models.Create;

namespace HomeAccounting.Domain.Validators;

internal class CreateSpendingValidation : AbstractValidator<CreateSpendingModel>
{
    public CreateSpendingValidation(IValidationService validationService)
    {
        RuleFor(createSpendingModel => createSpendingModel.UserId)
            .MustAsync(validationService.IsUserExistsAsync)
            .WithStatusCode(StatusCode.AmountMustBeGreaterThanZero)
            .Must(validationService.IsUserIsCurrentUser)
            .WithStatusCode(StatusCode.YouCanOnlyAddSpendingForYourself);

        RuleFor(createSpendingModel => createSpendingModel.Amount)
            .GreaterThan(0)
            .WithStatusCode(StatusCode.AmountMustBeGreaterThanZero);

        RuleFor(createSpendingModel => createSpendingModel.Description)
            .MaximumLength(250)
            .WithStatusCode(StatusCode.DescriptionTooLong);
    }
}