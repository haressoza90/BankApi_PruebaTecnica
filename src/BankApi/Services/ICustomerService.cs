using BankApi.Models;

namespace BankApi.Services
{
    public interface ICustomerService
    {
        Task<Customer> CreateAsync(Customer customer);
        Task<Customer?> GetAsync(int id);

        //para eliminar cliente por id
        Task<bool> DeleteAsync(int id);
    }
}
