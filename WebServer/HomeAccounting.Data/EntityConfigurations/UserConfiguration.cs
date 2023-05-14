#nullable disable

using HomeAccounting.Data.Entities;
using HomeAccounting.Data.Enums;
using HomeAccounting.Data.Enums.RichEnums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAccounting.Data.EntityConfigurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(user => user.Id);

        builder
            .ToTable(TableName.Users, TableSchema.Dbo);

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder
            .Property(user => user.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid)
            .IsRequired();

        builder
            .Property(user => user.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc)
            .IsRequired();

        builder
            .Property(user => user.UpdatedAt)
            .IsRequired(false);

        builder
            .Property(user => user.InvitationToken)
            .IsRequired(false);

        builder
            .Property(user => user.VerificationCode)
            .IsRequired(false);

        builder
            .Property(user => user.FirstName)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(user => user.LastName)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(user => user.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Ignore(user => user.FullName);

        builder
            .Property(user => user.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(user => user.ImageDataUrl)
            .HasMaxLength(255)
            .IsRequired(false);

        builder
            .Property(user => user.MonobankToken)
            .HasMaxLength(255)
            .IsRequired(false);

        builder
            .Property(user => user.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasDefaultValue(UserStatus.Pending)
            .IsRequired();

        builder
            .HasMany(user => user.Spendings)
            .WithOne(spending => spending.User)
            .HasForeignKey(spending => spending.UserId)
            .HasPrincipalKey(user => user.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(user => user.Incomings)
            .WithOne(incoming => incoming.User)
            .HasForeignKey(incoming => incoming.UserId)
            .HasPrincipalKey(user => user.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}