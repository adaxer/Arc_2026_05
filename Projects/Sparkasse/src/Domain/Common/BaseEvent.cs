using MediatR;

namespace Sparkasse.Domain.Common;

public abstract record BaseEvent : INotification
{
}
