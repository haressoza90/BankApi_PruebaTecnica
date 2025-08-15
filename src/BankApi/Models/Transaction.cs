namespace BankApi.Models;
public enum TransactionType { Deposit = 1, Withdrawal = 2, Interest = 3 }
public class Transaction
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public Account? Account { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public decimal BalanceAfter { get; set; }
}