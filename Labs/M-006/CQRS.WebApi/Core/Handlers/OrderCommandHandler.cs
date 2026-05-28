using CQRS.WebApi.Core.Commands;
using CQRS.WebApi.Core.Repositories;
using MediatR;

namespace CQRS.WebApi.Core.Handlers
{
    public class OrderCommandHandler : 
        IRequestHandler<PlaceOrderCommand, Guid>,
        IRequestHandler<CancelOrderCommand>
    {
        private readonly IOrdersRepository _repository;
        private readonly ILogger<OrderCommandHandler> _logger;

        public OrderCommandHandler(IOrdersRepository repository, ILogger<OrderCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Guid> Handle(PlaceOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = await _repository.PlaceOrderAsync(command.CustomerId, command.ProductId);
            _logger.LogInformation($"Created order for customer {command.CustomerId} for product {command.ProductId}");

            return orderId;
        }

        public async Task Handle(CancelOrderCommand command, CancellationToken cancellationToken)
        {
            await _repository.CancelOrderAsync(command.OrderId);
            _logger.LogInformation($"Cancelled order {command.OrderId}");
        }
    }
}
