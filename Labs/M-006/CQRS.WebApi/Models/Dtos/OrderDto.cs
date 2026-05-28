namespace CQRS.WebApi.Models.Dtos
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }

        public CustomerDto? Customer { get; set; }

        public ProductDto? Product { get; set; }

        public DateTime DeliveryDate { get; set; }

        public bool Delivered { get; set; }

        public bool Cancelled { get; set; }
    }
}