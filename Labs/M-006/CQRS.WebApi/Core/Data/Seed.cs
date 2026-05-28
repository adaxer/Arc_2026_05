using CQRS.WebApi.Models.Domain;

namespace CQRS.WebApi.Core.Data
{
    public class Seed
    {
        const string OrderId = "BD300B3B-9640-45C5-8BFF-F803FE98419F";
        const string ProductId = "FF2A9260-44AC-4D07-9E50-F9F75B842E03";
        const string CustomerId = "8992C295-AD57-4486-A082-235A0CDA0384";

        public static IEnumerable<Order> SeedOrders()
        {
            var customer = new Customer
            {
                CustomerId = Guid.Parse(CustomerId),
                Name = "John Doe"
            };
            var product = new Product
            {
                ProductId = Guid.Parse(ProductId),
                Name = "Awesome Widget 0815",
                Price = 99.99m
            };

            yield return new Order
            {
                OrderId = Guid.Parse(OrderId),
                CustomerId = customer.CustomerId,
                Customer = customer,
                ProductId = product.ProductId,
                Product = product
            };
        }
    }
}
