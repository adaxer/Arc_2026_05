namespace CQRS.WebApi.Models.Domain
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public DateTime DeliveryDate { get; set; }

        public bool Cancelled { get; set; }
    }
}