using Sparkasse.Application.BankAccounts.Queries;
using Sparkasse.Domain.Aggregates;
using Sparkasse.Domain.Events;

namespace Sparkasse.Application.BankAccounts;

public class EventStore
{
    private readonly Dictionary<Guid, List<BankAccountValue>> _store = new();

    private readonly Dictionary<Guid, BankAccount> _projection = new();

    public void Save(Guid aggregateId, params BankAccountValue[] events)
    {
        if (!_store.ContainsKey(aggregateId))
            _store[aggregateId] = [];

        _store[aggregateId].AddRange(events);

        _projection[aggregateId] = LoadAccount(aggregateId);
    }

    public IEnumerable<BankAccountValue> GetTransactions(Guid aggregateId)
        => _store.GetValueOrDefault(aggregateId) ?? Enumerable.Empty<BankAccountValue>();

    public BankAccount LoadAccount(Guid aggregateId)
    {
        var account = new BankAccount(aggregateId);
        if (_store.TryGetValue(aggregateId, out var events))
        {
            foreach (var value in events.OrderBy(e => e.Timestamp))
            {
                // BankAccountValue kann nur Deposits/Withdrawals repräsentieren
                // da es keine Informationen über den Event-Typ enthält
                if (value.Amount.HasValue)
                {   
                    var evt = value.Amount.Value >= 0
                        ? new MoneyDepositedEvent(aggregateId, value.Amount.Value) as BankAccountEvent
                        : new MoneyWithdrawnEvent(aggregateId, -value.Amount.Value);

                    account.Apply(evt);
                }
            }
        }
        return account;
    }

    public BankAccount? GetAccountView(Guid aggregateId)
        => _projection.GetValueOrDefault(aggregateId);
}
