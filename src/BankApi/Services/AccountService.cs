using BankApi.Data;
using BankApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Services;
public class AccountService : IAccountService
{
    private readonly AppDbContext _db;
    private readonly ITransactionService _txService;
    private readonly IInterestService _interest;

    public AccountService(AppDbContext db, ITransactionService txService, IInterestService interest)
    {
        _db = db;
        _txService = txService;
        _interest = interest;
    }

    public async Task<Account> CreateAccountAsync(int customerId, string accountNumber, decimal initialBalance)
    {
        var existsCustomer = await _db.Customers.AnyAsync(c => c.Id == customerId);
        if (!existsCustomer) throw new InvalidOperationException("Customer not found");
        if (await _db.Accounts.AnyAsync(a => a.AccountNumber == accountNumber))
            throw new InvalidOperationException("Account number already exists");

        var account = new Account { CustomerId = customerId, AccountNumber = accountNumber, Balance = 0m };
        _db.Accounts.Add(account);
        await _db.SaveChangesAsync();

        if (initialBalance > 0) await _txService.DepositAsync(accountNumber, initialBalance);
        return account;
    }

    public async Task<decimal> GetBalanceAsync(string accountNumber)
    {
        var acc = await _db.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber)
                  ?? throw new InvalidOperationException("Account not found");
        return acc.Balance;
    }

    public async Task ApplyInterestAsync(string accountNumber, decimal rate)
    {
        if (rate < 0) throw new ArgumentOutOfRangeException(nameof(rate));
        var acc = await _db.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber)
                  ?? throw new InvalidOperationException("Account not found");
        var interest = _interest.Calculate(acc.Balance, rate);
        if (interest == 0) return;
        await _txService.DepositAsync(accountNumber, interest, BankApi.Models.TransactionType.Interest);
    }

    // pa eliminar cuenta nomas
    public async Task<bool> DeleteAccountAsync(string accountNumber)
    {
        var account = await _db.Accounts
            .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

        if (account == null)
            return false;

        _db.Accounts.Remove(account);
        await _db.SaveChangesAsync();

        return true;
    }
}
