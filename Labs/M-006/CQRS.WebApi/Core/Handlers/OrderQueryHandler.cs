using CQRS.WebApi.Core.Queries;
using CQRS.WebApi.Core.Repositories;
using CQRS.WebApi.Mappers;
using CQRS.WebApi.Models.Dtos;
using MediatR;

namespace CQRS.WebApi.Core.Handlers
{
    public class OrderQueryHandler : 
        IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderDto>>,
        IRequestHandler<GetOrderByIdQuery, OrderDto?>
    {
        private readonly IOrdersRepository _repository;

        public OrderQueryHandler(IOrdersRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _repository.GetOrdersAsync();
            return orders.Select(o => o.ToDto());
        }

        public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetOrderAsync(request.OrderId);
            return order?.ToDto();
        }
    }
}
