namespace HomeAccounting.Models.Views;

public class CreditView
{
    public Guid Id { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public DateTime CreditDate { get; set; }

    public DateTime? CreditUpdatedAt { get; set; }

    public UserView? User { get; set; }
}