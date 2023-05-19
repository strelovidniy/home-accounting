namespace HomeAccounting.Models.Update;

public class UpdateCreditModel : IValidatableModel
{
    public Guid CreditId { get; set; }

    public string? Description { get; set; }
}