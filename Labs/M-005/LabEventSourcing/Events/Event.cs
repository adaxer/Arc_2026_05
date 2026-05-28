namespace LabEventSourcing.Events;

public abstract record Event(Guid AggregateId, DateTime Timestamp);

public record AccountOpenedEvent(
    Guid AggregateId,
    decimal InitialBalance) : Event(AggregateId, DateTime.UtcNow);

public record MoneyDepositedEvent(
    Guid AggregateId,
    decimal Amount) : Event(AggregateId, DateTime.UtcNow);

public record MoneyWithdrawnEvent(
    Guid AggregateId,
    decimal Amount) : Event(AggregateId, DateTime.UtcNow);

public record AccountClosedEvent(Guid AggregateId) : Event(AggregateId, DateTime.UtcNow);
