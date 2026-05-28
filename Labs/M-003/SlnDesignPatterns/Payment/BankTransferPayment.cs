using PaymentService.Contracts;

namespace PaymentService.Payment;

public class BankTransferPayment : IPaymentStrategy
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paying {amount:C2} using Bank Transfer");
    }
}
