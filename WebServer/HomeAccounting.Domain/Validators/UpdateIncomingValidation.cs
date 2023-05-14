using FluentValidation;
using HomeAccounting.Data.Enums;
using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Domain.Validators.Extensions;
using HomeAccounting.Models.Update;

namespace HomeAccounting.Domain.Validators;

internal class UpdateIncomingValidation : AbstractValidator<UpdateIncomingModel>
{
    public UpdateIncomingValidation(IValidationService validationService)
    {
        RuleFor(updateIncomingModel => updateIncomingModel.IncomingId)
            .MustAsync(validationService.IsIncomingExistAsync)
            .WithStatusCode(StatusCode.IncomingNotFound)
            .MustAsync(validationService.IsIncomingBelongsToCurrentUserAsync)
            .WithStatusCode(StatusCode.YouCanOnlyUpdateIncomingForYourself);

        RuleFor(updateIncomingModel => updateIncomingModel.Description)
            .MaximumLength(250)
            .WithStatusCode(StatusCode.DescriptionTooLong);
    }
}