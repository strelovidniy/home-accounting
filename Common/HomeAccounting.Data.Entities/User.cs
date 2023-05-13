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

    public string? PasswordHash { get; set; }
}