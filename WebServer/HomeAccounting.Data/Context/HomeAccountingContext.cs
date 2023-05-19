#nullable enable

using HomeAccounting.Data.Entities;
using HomeAccounting.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Data.Context;

internal class HomeAccountingContext : DbContext
{
    public virtual DbSet<User> Users { get; set; } = null!;

    public virtual DbSet<Spending> Spendings { get; set; } = null!;

    public virtual DbSet<Incoming> Incomings { get; set; } = null!;
    public virtual DbSet<Credit> Credits { get; set; } = null!;

    public HomeAccountingContext(DbContextOptions<HomeAccountingContext> options)
        : base(options)
    {
    }

    public HomeAccountingContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new SpendingConfiguration());
        modelBuilder.ApplyConfiguration(new IncomingConfiguration());
        modelBuilder.ApplyConfiguration(new CreditConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}