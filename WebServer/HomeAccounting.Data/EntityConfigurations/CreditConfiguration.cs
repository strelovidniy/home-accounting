using HomeAccounting.Data.Entities;
using HomeAccounting.Data.Enums.RichEnums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAccounting.Data.EntityConfigurations;

public class CreditConfiguration : IEntityTypeConfiguration<Credit>
{
    public void Configure(EntityTypeBuilder<Credit> builder)
    {
        builder
            .HasKey(credit => credit.Id);

        builder
            .ToTable(TableName.Credits, TableSchema.Dbo);

        builder
            .Property(credit => credit.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid)
            .IsRequired();

        builder
            .Property(credit => credit.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc)
            .IsRequired();

        builder
            .Property(credit => credit.UpdatedAt)
            .IsRequired(false);

        builder
            .Property(credit => credit.Amount)
            .IsRequired();

        builder
            .Property(credit => credit.Description)
            .HasMaxLength(255)
            .IsRequired(false);
    }
}