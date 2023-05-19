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
            .HasKey(deposit => deposit.Id);

        builder
            .ToTable(TableName.Deposits, TableSchema.Dbo);

        builder
            .Property(deposit => deposit.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid)
            .IsRequired();

        builder
            .Property(deposit => deposit.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc)
            .IsRequired();

        builder
            .Property(deposit => deposit.UpdatedAt)
            .IsRequired(false);

        builder
            .Property(deposit => deposit.Amount)
            .IsRequired();

        builder
            .Property(deposit => deposit.Description)
            .HasMaxLength(255)
            .IsRequired(false);
    }
}