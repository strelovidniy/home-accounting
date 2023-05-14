namespace HomeAccounting.Models.Views;

public class IncomingView
{
    public Guid Id { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public DateTime IncomingDate { get; set; }

    public DateTime? IncomingUpdatedAt { get; set; }

    public UserView? User { get; set; }
}