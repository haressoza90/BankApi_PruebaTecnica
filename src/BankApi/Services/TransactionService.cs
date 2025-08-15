using BankApi.Data;
using BankApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Services;
public class TransactionService : ITransactionService
{
    //implementacion de transaccioes, aqui es 
    //y me sirve para manejar toda la logica de depositos, retiros y obtencion del historial
    private readonly AppDbContext _db;
    public TransactionService(AppDbContext db) => _db = db;

    public async Task DepositAsync(string accountNumber, decimal amount, TransactionType type = TransactionType.Deposit)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        var acc = await _db.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber)
                  ?? throw new InvalidOperationException("Account not found");
        acc.Balance += amount;
        var tx = new Transaction { AccountId = acc.Id, Type = type, Amount = amount, Date = DateTime.UtcNow, BalanceAfter = acc.Balance };
        _db.Transactions.Add(tx);
        await _db.SaveChangesAsync();
    }

    public async Task WithdrawAsync(string accountNumber, decimal amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        var acc = await _db.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber)
                  ?? throw new InvalidOperationException("Account not found");
        if (acc.Balance < amount) throw new InvalidOperationException("Insufficient funds");
        acc.Balance -= amount;
        var tx = new Transaction { AccountId = acc.Id, Type = TransactionType.Withdrawal, Amount = amount, Date = DateTime.UtcNow, BalanceAfter = acc.Balance };
        _db.Transactions.Add(tx);
        await _db.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Transaction>> GetTransactionsAsync(string accountNumber)
    {
        var acc = await _db.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber)
                  ?? throw new InvalidOperationException("Account not found");
        return await _db.Transactions.Where(t => t.AccountId == acc.Id).OrderBy(t => t.Date).ThenBy(t => t.Id).ToListAsync();
    }
}