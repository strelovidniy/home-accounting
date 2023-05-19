namespace HomeAccounting.Models.Create;

public class CreateDepositModel : IValidatableModel
{
    public decimal Amount { get; set; }

    public int RateOfInterest { get; set; }

    public int NumberOfYears { get; set; }

    public string? Description { get; set; }

    public Guid UserId { get; set; }
}