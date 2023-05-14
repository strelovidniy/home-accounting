using FluentValidation;
using HomeAccounting.Data.Enums;
using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Domain.Validators.Extensions;
using HomeAccounting.Models.Update;

namespace HomeAccounting.Domain.Validators;

internal class UpdateSpendingValidation : AbstractValidator<UpdateSpendingModel>
{
    public UpdateSpendingValidation(IValidationService validationService)
    {
        RuleFor(updateSpendingModel => updateSpendingModel.SpendingId)
            .MustAsync(validationService.IsSpendingExistAsync)
            .WithStatusCode(StatusCode.SpendingNotFound)
            .MustAsync(validationService.IsSpendingBelongsToCurrentUserAsync)
            .WithStatusCode(StatusCode.YouCanOnlyUpdateSpendingForYourself);

        RuleFor(updateSpendingModel => updateSpendingModel.Description)
            .MaximumLength(250)
            .WithStatusCode(StatusCode.DescriptionTooLong);
    }
}