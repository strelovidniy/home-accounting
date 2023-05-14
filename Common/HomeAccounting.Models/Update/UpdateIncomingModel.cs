namespace HomeAccounting.Models.Update;

public class UpdateIncomingModel : IValidatableModel
{
    public Guid IncomingId { get; set; }

    public string? Description { get; set; }
}