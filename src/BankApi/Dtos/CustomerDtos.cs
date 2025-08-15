namespace BankApi.Dtos;
public record CreateCustomerDto(string Name, DateTime BirthDate, string Sex, decimal Income);
public record CustomerResponse(int Id, string Name, DateTime BirthDate, string Sex, decimal Income);