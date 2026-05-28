using CQRS.WebApi.Models.Domain;
using CQRS.WebApi.Models.Dtos;

namespace CQRS.WebApi.Mappers
{
    public static class Mappers
    {
        public static OrderDto? ToDto(this Order order)
        {
            return order is null ? null : new OrderDto
            {
                OrderId = order.OrderId,
                Customer = order.Customer.ToDto(),
                Product = order.Product.ToDto(),
                DeliveryDate = order.DeliveryDate,
                Delivered = !order.DeliveryDate.Equals(default),
                Cancelled = order.Cancelled,
            };
        }

        public static CustomerDto? ToDto(this Customer customer)
        {
            return customer is null ? null : new CustomerDto
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name
            };
        }

        public static ProductDto? ToDto(this Product product)
        {
            return product is null ? null : new ProductDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price
            };
        }
    }
}
