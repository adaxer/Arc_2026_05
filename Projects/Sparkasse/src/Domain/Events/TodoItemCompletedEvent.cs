namespace Sparkasse.Domain.Events;

public record TodoItemCompletedEvent(TodoItem Item) : BaseEvent;
