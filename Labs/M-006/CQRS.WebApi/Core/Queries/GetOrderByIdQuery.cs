using CQRS.WebApi.Models.Dtos;
using MediatR;

namespace CQRS.WebApi.Core.Queries
{
    public record GetOrderByIdQuery : IRequest<OrderDto>
    {
        public Guid OrderId { get; }

        public GetOrderByIdQuery(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
