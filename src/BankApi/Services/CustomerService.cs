using BankApi.Data;
using BankApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Services;
public class CustomerService : ICustomerService
{
    private readonly AppDbContext _db;
    public CustomerService(AppDbContext db) => _db = db;

    public async Task<Customer> CreateAsync(Customer customer)
    {
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();
        return customer;
    }

    public Task<Customer?> GetAsync(int id) =>
        _db.Customers.Include(c => c.Accounts).FirstOrDefaultAsync(c => c.Id == id);

    // esto es para eliminar cliente si asi lo quiero
    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _db.Customers
            .FirstOrDefaultAsync(c => c.Id == id);

        if (customer == null)
            return false;

        _db.Customers.Remove(customer);
        await _db.SaveChangesAsync();

        // Si ya no quedan clientes, esto es para reiniciar el contador a 0
        if (!await _db.Customers.AnyAsync())
        {
            await ResetCustomerIdSequenceAsync();
        }

        return true;
    }

    // esto es para el autoincremento en esta tabla nomas
    private async Task ResetCustomerIdSequenceAsync()
    {
        await _db.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name = 'Customers';");
    }
}
