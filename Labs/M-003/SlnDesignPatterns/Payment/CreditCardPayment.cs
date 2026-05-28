using PaymentService.Contracts;

namespace PaymentService.Payment;

public class CreditCardPayment : IPaymentStrategy
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paying {amount:C2} using Credit Card");
    }
}
