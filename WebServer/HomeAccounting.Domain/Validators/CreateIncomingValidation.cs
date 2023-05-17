using FluentValidation;
using HomeAccounting.Data.Enums;
using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Domain.Validators.Extensions;
using HomeAccounting.Models.Create;

namespace HomeAccounting.Domain.Validators;

internal class CreateIncomingValidation : AbstractValidator<CreateIncomingModel>
{
    public CreateIncomingValidation(IValidationService validationService)
    {
        RuleFor(createIncomingModel => createIncomingModel.UserId)
            .MustAsync(validationService.IsUserExistsAsync)
            .WithStatusCode(StatusCode.UserNotFound)
            .Must(validationService.IsUserIsCurrentUser)
            .WithStatusCode(StatusCode.YouCanOnlyAddIncomingForYourself);

        RuleFor(createIncomingModel => createIncomingModel.Amount)
            .GreaterThan(0)
            .WithStatusCode(StatusCode.AmountMustBeGreaterThanZero);

        RuleFor(createIncomingModel => createIncomingModel.Description)
            .MaximumLength(250)
            .WithStatusCode(StatusCode.DescriptionTooLong);
    }
}