using HomeAccounting.Data.Entities;
using HomeAccounting.Data.Enums.RichEnums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAccounting.Data.EntityConfigurations;

public class DepositConfiguration : IEntityTypeConfiguration<Deposit>
{
    public void Configure(EntityTypeBuilder<Deposit> builder)
    {
        builder
            .HasKey(Deposit => Deposit.Id);

        builder
            .ToTable(TableName.Deposits, TableSchema.Dbo);

        builder
            .Property(Deposit => Deposit.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid)
            .IsRequired();

        builder
            .Property(Deposit => Deposit.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc)
            .IsRequired();

        builder
            .Property(Deposit => Deposit.UpdatedAt)
            .IsRequired(false);

        builder
            .Property(Deposit => Deposit.Amount)
            .IsRequired();

        builder
            .Property(Deposit => Deposit.Description)
            .HasMaxLength(255)
            .IsRequired(false);
    }
}