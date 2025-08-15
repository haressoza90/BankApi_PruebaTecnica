using System;
using System.Threading.Tasks;
using BankApi.Data;
using BankApi.Models;
using BankApi.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BankApi.Tests
{
    public class AccountServiceTests
    {
        private static AppDbContext CreateDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task Create_Account_Succeeds()
        {
            using var db = CreateDb();
            db.Customers.Add(new Customer { Id = 1, Name = "Alice", BirthDate = new DateTime(1990, 1, 1), Sex = "F", Income = 1000 });
            await db.SaveChangesAsync();

            var tx = new TransactionService(db);
            var svc = new AccountService(db, tx, new InterestService());
            var acc = await svc.CreateAccountAsync(1, "ACC-001", 100m);
            acc.AccountNumber.Should().Be("ACC-001");
            (await svc.GetBalanceAsync("ACC-001")).Should().Be(100m);
        }

        [Fact]
        public async Task Deposit_And_Withdraw_Work_Correctly()
        {
            using var db = CreateDb();
            db.Customers.Add(new Customer { Id = 1, Name = "Bob", BirthDate = new DateTime(1985, 5, 5), Sex = "M", Income = 2000 });
            await db.SaveChangesAsync();

            var tx = new TransactionService(db);
            var svc = new AccountService(db, tx, new InterestService());
            await svc.CreateAccountAsync(1, "ACC-002", 50m);

            await tx.DepositAsync("ACC-002", 25m);
            (await svc.GetBalanceAsync("ACC-002")).Should().Be(75m);

            await tx.WithdrawAsync("ACC-002", 25m);
            (await svc.GetBalanceAsync("ACC-002")).Should().Be(50m);
        }

        [Fact]
        public async Task Withdraw_With_Insufficient_Funds_Is_Rejected()
        {
            using var db = CreateDb();
            db.Customers.Add(new Customer { Id = 1, Name = "Carol", BirthDate = new DateTime(1995, 3, 3), Sex = "F", Income = 1500 });
            await db.SaveChangesAsync();

            var tx = new TransactionService(db);
            var svc = new AccountService(db, tx, new InterestService());
            await svc.CreateAccountAsync(1, "ACC-003", 10m);

            Func<Task> act = async () => await tx.WithdrawAsync("ACC-003", 20m);
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*Insufficient funds*");
        }

        [Fact]
        public async Task Apply_Interest_Increases_Balance_And_Registers_Transaction()
        {
            using var db = CreateDb();
            db.Customers.Add(new Customer { Id = 1, Name = "Dave", BirthDate = new DateTime(1980, 2, 2), Sex = "M", Income = 3000 });
            await db.SaveChangesAsync();

            var tx = new TransactionService(db);
            var svc = new AccountService(db, tx, new InterestService());
            await svc.CreateAccountAsync(1, "ACC-004", 100m);

            await svc.ApplyInterestAsync("ACC-004", 0.05m);
            (await svc.GetBalanceAsync("ACC-004")).Should().Be(105m);

            var txs = await tx.GetTransactionsAsync("ACC-004");
            txs.Should().ContainSingle(t => t.Type == TransactionType.Interest && t.Amount == 5m && t.BalanceAfter == 105m);
        }

        [Fact]
        public async Task Get_Transactions_Returns_History_With_BalanceAfter()
        {
            using var db = CreateDb();
            db.Customers.Add(new Customer { Id = 1, Name = "Eve", BirthDate = new DateTime(2000, 1, 1), Sex = "F", Income = 1200 });
            await db.SaveChangesAsync();

            var tx = new TransactionService(db);
            var svc = new AccountService(db, tx, new InterestService());
            await svc.CreateAccountAsync(1, "ACC-005", 0m);

            await tx.DepositAsync("ACC-005", 10m);
            await tx.DepositAsync("ACC-005", 20m);
            await tx.WithdrawAsync("ACC-005", 5m);

            var txs = await tx.GetTransactionsAsync("ACC-005");
            txs.Should().HaveCount(3);
            txs[0].BalanceAfter.Should().Be(10m);
            txs[1].BalanceAfter.Should().Be(30m);
            txs[2].BalanceAfter.Should().Be(25m);
        }
    }
}