namespace HomeAccounting.Models.Views;

public class DepositView
{
    public Guid Id { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public DateTime DepositDate { get; set; }

    public DateTime? DepositUpdatedAt { get; set; }
    
    public int DepositRateOfInterest { get; set; } // in percentage
    public int DepositNumberOfYears { get; set; } 
    public int DepositCompoundingFrequency { get; set; } 

    public UserView? User { get; set; }
}