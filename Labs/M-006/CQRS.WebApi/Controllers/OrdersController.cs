using CQRS.WebApi.Core.Commands;
using CQRS.WebApi.Core.Queries;
using CQRS.WebApi.Core.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrdersRepository _repository;
        private readonly IMediator _mediator;

        public OrdersController(ILogger<OrdersController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            //var orders = await _repository.GetOrdersAsync();
            //return Ok(orders);

            var query = new GetAllOrdersQuery();
            var result = await _mediator.Send(query, token);
            return Ok(result);
        }

        [HttpGet("{orderId:guid}")]
        public async Task<IActionResult> Get(Guid orderId, CancellationToken token)
        {
            var query = new GetOrderByIdQuery(orderId);
            var result = await _mediator.Send(query, token);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost("place")]
        public async Task<IActionResult> Place([FromBody] PlaceOrderCommand command, CancellationToken token)
        {
            //var order = await _repository.CreateOrderAsync(command.CustomerId, command.ProductId);
            //_logger.LogInformation($"Created order for customer {order.CustomerId} for product {order.ProductId}");

            var result = await _mediator.Send(command, token);
            return CreatedAtAction(nameof(Get), new { orderId = result }, result);
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> Cancel([FromBody] CancelOrderCommand command, CancellationToken token)
        {
            await _mediator.Send(command, token);
            return Ok();
        }
    }
}
