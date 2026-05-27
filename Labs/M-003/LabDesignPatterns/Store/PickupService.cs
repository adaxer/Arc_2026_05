namespace PaymentService.Store
{
    public class PickupService
    {
        private decimal _balance;

        public Person Customer { get; }

        public PickupService(Person customer)
        {
            Customer = customer;
        }

        public void PayWithCash(decimal amount)
        {
            _balance = Customer.GetCash() - amount;
        }

        public decimal ReturnChange()
        {
            if (_balance < 0)
            {
                throw new InvalidOperationException("Not enough cash");
            }
            return _balance;
        }
    }
}
