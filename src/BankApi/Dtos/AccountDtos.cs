namespace BankApi.Dtos;
public record CreateAccountDto(int CustomerId, string AccountNumber, decimal InitialBalance);
public record BalanceResponse(string AccountNumber, decimal Balance);
public record ApplyInterestDto(decimal Rate);