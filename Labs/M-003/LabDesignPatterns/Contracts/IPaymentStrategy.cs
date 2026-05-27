namespace PaymentService.Contracts
{
    public interface IPaymentStrategy
    {
        void Pay(decimal amount);
    }
}
