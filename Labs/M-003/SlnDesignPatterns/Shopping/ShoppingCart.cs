using PaymentService.Contracts;
using PaymentService.Payment;
using System.Text;

namespace PaymentService.Shopping;

public record Product(string Name, decimal Price);

public class ShoppingCart
{
    private IPaymentStrategy? _paymentStrategy;
    private List<Product> _products = new List<Product>();

    public void SetPaymentMethod(PaymentMethod paymentMethod)
    {
        _paymentStrategy = CreatePaymentStrategy(paymentMethod);
    }

    private IPaymentStrategy CreatePaymentStrategy(PaymentMethod paymentMethod)
    {
        return paymentMethod switch 
        { 
            PaymentMethod.PayPal => new PayPalPayment(), 
            PaymentMethod.CreditCard => new CreditCardPayment(), 
            PaymentMethod.BankTransfer => new BankTransferPayment(), 
            PaymentMethod.SelfPickup => new SelfPickupPaymentAdapter(), 
            _ => throw new ArgumentException("Invalid payment method") 
        };
    }

    public void AddProduct(Product product)
    {
        _products.Add(product);
    }

    public decimal GetTotalAmount()
    {
        return _products.Sum(p => p.Price);
    }

    public void Pay()
    {
        if (_paymentStrategy == null)
        {
            throw new InvalidOperationException("Payment strategy is not set");
        }

        decimal totalAmount = GetTotalAmount();
        _paymentStrategy.Pay(totalAmount);
    }

    public void PrintInfo()
    {
        var info = _products.Aggregate(new StringBuilder(), (sb, p) => sb.AppendLine($"{p.Name}\t {p.Price:C2}"));
        Console.Write(info);
    }
}
