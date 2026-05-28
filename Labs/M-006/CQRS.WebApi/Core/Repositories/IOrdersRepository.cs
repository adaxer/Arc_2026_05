using CQRS.WebApi.Models.Domain;

namespace CQRS.WebApi.Core.Repositories
{
    public interface IOrdersRepository
    {
        Task<Guid> PlaceOrderAsync(Guid customerId, Guid productId);

        Task CancelOrderAsync(Guid orderId);

        Task<Order?> GetOrderAsync(Guid orderId);

        Task<IEnumerable<Order>> GetOrdersAsync();
    }
}
