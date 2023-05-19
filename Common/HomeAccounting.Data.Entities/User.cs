using EntityFrameworkCore.RepositoryInfrastructure;
using HomeAccounting.Data.Enums;

namespace HomeAccounting.Data.Entities;

public class User : Entity, IEntity
{
    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string FullName => $"{FirstName} {LastName}";

    public UserStatus Status { get; set; }

    public Guid? InvitationToken { get; set; }

    public Guid? VerificationCode { get; set; }

    public Guid? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpirationDate { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string? ImageDataUrl { get; set; }

    public string? MonobankToken { get; set; }

    public IEnumerable<Spending> Spendings { get; set; } = new List<Spending>();

    public IEnumerable<Incoming> Incomings { get; set; } = new List<Incoming>();
    public IEnumerable<Credit> Credits { get; set; } = new List<Credit>();
}