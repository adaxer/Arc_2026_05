namespace Sparkasse.Domain.Events;

public abstract record BankAccountEvent(Guid AggregateId, DateTime Timestamp) : BaseEvent;

public record AccountOpenedEvent(
    Guid AggregateId,
    decimal InitialBalance) : BankAccountEvent(AggregateId, DateTime.UtcNow);

public record MoneyDepositedEvent(
    Guid AggregateId,
    decimal Amount) : BankAccountEvent(AggregateId, DateTime.UtcNow);

public record MoneyWithdrawnEvent(
    Guid AggregateId,
    decimal Amount) : BankAccountEvent(AggregateId, DateTime.UtcNow);

public record AccountClosedEvent(Guid AggregateId) : BankAccountEvent(AggregateId, DateTime.UtcNow);
