using EntityFrameworkCore.RepositoryInfrastructure;

namespace HomeAccounting.Data.Entities;

public class Deposit : Entity, IEntity
{
    public Guid UserId { get; set; }

    public decimal Amount { get; set; }

    public int RateOfInterest { get; set; } // in percentage

    public int NumberOfYears { get; set; }

    public string? Description { get; set; }

    public User? User { get; set; }
}