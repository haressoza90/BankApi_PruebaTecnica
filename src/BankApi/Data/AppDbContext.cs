using BankApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>().HasIndex(a => a.AccountNumber).IsUnique();
        modelBuilder.Entity<Account>().Property(a => a.Balance).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Transaction>().Property(t => t.Amount).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Transaction>().Property(t => t.BalanceAfter).HasColumnType("decimal(18,2)");
    }
}