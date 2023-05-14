namespace HomeAccounting.Models.Update;

public class UpdateSpendingModel : IValidatableModel
{
    public Guid SpendingId { get; set; }

    public string? Description { get; set; }
}