using PaymentService.Contracts;
using PaymentService.Store;

namespace PaymentService.Payment;

public class SelfPickupPaymentAdapter : IPaymentStrategy
{
    private readonly Person _customer = new Person();

    public void Pay(decimal amount)
    {
        var _pickupService = new PickupService(_customer);

        try
        {
            _pickupService.PayWithCash(amount);
            _pickupService.ReturnChange();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message + ": " + _customer.GetCash());
        }

        Console.WriteLine($"Paying {amount:C2} with cash at the store.");
    }
}
