using CQRS.WebApi.Core.Data;
using CQRS.WebApi.Models.Domain;

namespace CQRS.WebApi.Core.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly Dictionary<Guid, Order> _orders = Seed.SeedOrders().ToDictionary(x => x.OrderId);

        public Task<Guid> PlaceOrderAsync(Guid customerId, Guid productId)
        {
            var orderId = Guid.NewGuid();
            _orders.Add(orderId, new Order
            {
                OrderId = orderId,
                ProductId = productId,
                CustomerId = customerId,
            });
            return Task.FromResult(orderId);
        }

        public async Task CancelOrderAsync(Guid orderId)
        {
            var order = await GetOrderAsync(orderId);
            if (order != null)
            {
                order.Cancelled = true;
            }
        }

        public Task<Order?> GetOrderAsync(Guid orderId)
        {
            return Task.FromResult(_orders.GetValueOrDefault(orderId));
        }

        public Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return Task.FromResult(_orders.Select(d => d.Value));
        }
    }
}
