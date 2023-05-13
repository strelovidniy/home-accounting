#nullable disable

using HomeAccounting.Data.Entities;
using HomeAccounting.Data.Enums;
using HomeAccounting.Data.Enums.RichEnums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAccounting.Data.EntityConfigurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity
            .HasKey(e => e.Id);

        entity
            .ToTable(TableName.Users, TableSchema.Dbo);

        entity.HasIndex(e => e.Email)
            .IsUnique();

        entity
            .Property(e => e.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid)
            .IsRequired();

        entity
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc)
            .IsRequired();

        entity
            .Property(e => e.UpdatedAt)
            .IsRequired(false);

        entity
            .Property(e => e.InvitationToken)
            .IsRequired(false);

        entity
            .Property(e => e.VerificationCode)
            .IsRequired(false);

        entity
            .Property(e => e.FirstName)
            .HasMaxLength(255)
            .IsRequired();

        entity
            .Property(e => e.LastName)
            .HasMaxLength(255)
            .IsRequired();

        entity
            .Property(e => e.Email)
            .HasMaxLength(255)
            .IsRequired();

        entity
            .Ignore(e => e.FullName);

        entity
            .Property(e => e.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();

        entity
            .Property(e => e.ImageDataUrl)
            .HasMaxLength(255)
            .IsRequired(false);

        entity
            .Property(user => user.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasDefaultValue(UserStatus.Pending)
            .IsRequired();
    }
}