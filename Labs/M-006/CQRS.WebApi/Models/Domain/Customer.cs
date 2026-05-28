namespace CQRS.WebApi.Models.Domain
{
    public class Customer
    {
        public Guid CustomerId { get; set; }

        public string Name { get; set; }
    }
}