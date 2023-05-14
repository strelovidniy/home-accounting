using HomeAccounting.Data.Entities;
using HomeAccounting.Data.Enums.RichEnums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAccounting.Data.EntityConfigurations;

internal class SpendingConfiguration : IEntityTypeConfiguration<Spending>
{
    public void Configure(EntityTypeBuilder<Spending> builder)
    {
        builder
            .HasKey(spending => spending.Id);

        builder
            .ToTable(TableName.Spendings, TableSchema.Dbo);

        builder
            .Property(spending => spending.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid)
            .IsRequired();

        builder
            .Property(spending => spending.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc)
            .IsRequired();

        builder
            .Property(spending => spending.UpdatedAt)
            .IsRequired(false);

        builder
            .Property(spending => spending.Amount)
            .IsRequired();

        builder
            .Property(spending => spending.Description)
            .HasMaxLength(255)
            .IsRequired(false);
    }
}