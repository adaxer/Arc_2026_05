using MediatR;

namespace CQRS.WebApi.Core.Commands
{
    public class PlaceOrderCommand : IRequest<Guid>
    {
        public Guid CustomerId { get; set; }

        public Guid ProductId { get; set; }
    }
}