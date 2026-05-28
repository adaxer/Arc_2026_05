using PaymentService.Contracts;

namespace PaymentService.Payment;

public class PayPalPayment : IPaymentStrategy
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paying {amount:C2} using PayPal");
    }
}
