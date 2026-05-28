using MediatR;

namespace CQRS.WebApi.Core.Commands
{
    public class CancelOrderCommand : IRequest
    {
        public Guid OrderId { get; set; }
    }
}