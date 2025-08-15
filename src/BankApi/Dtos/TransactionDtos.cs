namespace BankApi.Dtos;
public record MovementRequest(string AccountNumber, decimal Amount);