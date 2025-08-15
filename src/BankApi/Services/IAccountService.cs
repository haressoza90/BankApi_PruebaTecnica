using BankApi.Models;

namespace BankApi.Services
{
    public interface IAccountService
    {
        Task<Account> CreateAccountAsync(int customerId, string accountNumber, decimal initialBalance);
        Task<decimal> GetBalanceAsync(string accountNumber);
        Task ApplyInterestAsync(string accountNumber, decimal rate);

        //para eliminar cuenta por número
        Task<bool> DeleteAccountAsync(string accountNumber);
    }
}
