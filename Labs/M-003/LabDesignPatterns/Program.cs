using PaymentService.Payment;
using PaymentService.Shopping;

namespace PaymentService;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var cart = new ShoppingCartBuilder()
            .AddProduct("Laptop", 1000.0m)
            .AddProduct("Mouse", 50.0m)
            .SetPaymentMethod(PaymentMethod.CreditCard)
            .Build();

        cart.PrintInfo();
        cart.Pay();
        Console.WriteLine();

        cart = new ShoppingCartBuilder()
            .AddProduct("Smartphone", 800.0m)
            .AddProduct("Headphones", 100.0m)
            .SetPaymentMethod(PaymentMethod.PayPal)
            .Build();

        cart.PrintInfo();
        cart.Pay();
        Console.WriteLine();

        cart = new ShoppingCartBuilder()
            .AddProduct("Tablet", 500.0m)
            .AddProduct("Keyboard", 75.0m)
            .SetPaymentMethod(PaymentMethod.BankTransfer)
            .Build();

        cart.PrintInfo();
        cart.Pay();

        cart = new ShoppingCartBuilder()
            .AddProduct("Monitor", 300.0m)
            .AddProduct("Speaker", 150.0m)
            .SetPaymentMethod(PaymentMethod.SelfPickup)
            .Build();

        cart.Pay();
    }
}
