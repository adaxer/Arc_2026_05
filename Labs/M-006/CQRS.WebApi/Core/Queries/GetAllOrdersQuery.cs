using CQRS.WebApi.Models.Dtos;
using MediatR;

namespace CQRS.WebApi.Core.Queries
{
    // records are immutable
    public record GetAllOrdersQuery : IRequest<IEnumerable<OrderDto>>
    {
        public GetAllOrdersQuery()
        {
        }
    }
}
