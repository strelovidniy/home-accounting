namespace HomeAccounting.Models.Update;

public class UpdateDepositModel : IValidatableModel
{
    public Guid DepositId { get; set; }

    public string? Description { get; set; }
}