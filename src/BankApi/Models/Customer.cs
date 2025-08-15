namespace BankApi.Models;
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Sex { get; set; } = string.Empty;
    public decimal Income { get; set; }
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}