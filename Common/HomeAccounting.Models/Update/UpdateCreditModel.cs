namespace HomeAccounting.Models.Update;

public class UpdateDepositModel : IValidatableModel
{
    public Guid IncomingId { get; set; }

    public string? Description { get; set; }
}