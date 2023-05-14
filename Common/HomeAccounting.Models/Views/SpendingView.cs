namespace HomeAccounting.Models.Views;

public class SpendingView
{
    public Guid Id { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public DateTime SpendingDate { get; set; }

    public DateTime? SpendingUpdatedAt { get; set; }

    public UserView? User { get; set; }
}