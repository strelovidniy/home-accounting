using HomeAccounting.Data.Entities;
using HomeAccounting.Data.Enums.RichEnums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeAccounting.Data.EntityConfigurations;

internal class IncomingConfiguration : IEntityTypeConfiguration<Incoming>
{
    public void Configure(EntityTypeBuilder<Incoming> builder)
    {
        builder
            .HasKey(incoming => incoming.Id);

        builder
            .ToTable(TableName.Incomings, TableSchema.Dbo);

        builder
            .Property(incoming => incoming.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid)
            .IsRequired();

        builder
            .Property(incoming => incoming.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc)
            .IsRequired();

        builder
            .Property(incoming => incoming.UpdatedAt)
            .IsRequired(false);

        builder
            .Property(incoming => incoming.Amount)
            .IsRequired();

        builder
            .Property(incoming => incoming.Description)
            .HasMaxLength(255)
            .IsRequired(false);
    }
}