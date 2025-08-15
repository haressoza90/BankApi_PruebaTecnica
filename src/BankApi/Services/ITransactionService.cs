using BankApi.Models;
namespace BankApi.Services;
public interface ITransactionService
{
    //para las transaccioens
    Task DepositAsync(string accountNumber, decimal amount, TransactionType type = TransactionType.Deposit);
    Task WithdrawAsync(string accountNumber, decimal amount);
    Task<IReadOnlyList<Transaction>> GetTransactionsAsync(string accountNumber);
}