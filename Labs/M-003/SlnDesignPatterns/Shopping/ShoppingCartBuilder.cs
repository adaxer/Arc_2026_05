using PaymentService.Payment;

namespace PaymentService.Shopping;

public class ShoppingCartBuilder
{
    private ShoppingCart _shoppingCart;

    public ShoppingCartBuilder()
    {
        _shoppingCart = new ShoppingCart();
    }

    public ShoppingCartBuilder AddProduct(string name, decimal price)
    {
        _shoppingCart.AddProduct(new Product(name, price));
        return this;
    }

    public ShoppingCartBuilder SetPaymentMethod(PaymentMethod paymentMethod)
    {
        _shoppingCart.SetPaymentMethod(paymentMethod);
        return this;
    }

    public ShoppingCart Build()
    {
        return _shoppingCart;
    }
}
