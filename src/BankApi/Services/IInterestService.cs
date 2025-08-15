namespace BankApi.Services
{
    public interface IInterestService
    {
        decimal Calculate(decimal balance, decimal rate);
    }

    public class InterestService : IInterestService
    {
        public decimal Calculate(decimal balance, decimal rate)
        {
            if (rate < 0) throw new ArgumentOutOfRangeException(nameof(rate));

            // esto es para convertir rate a porcentaje, que para mi serian los intereses
            return Math.Round(balance * (rate / 100), 2);
        }
    }
}
