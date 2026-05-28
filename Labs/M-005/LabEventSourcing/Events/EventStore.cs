namespace LabEventSourcing.Events;

public class EventStore
{
    private readonly Dictionary<Guid, List<Event>> _store = new();

    private readonly Dictionary<Guid, BankAccount> _projection = new();

    public void Save(Guid aggregateId, params Event[] events)
    {
        if (!_store.ContainsKey(aggregateId))
            _store[aggregateId] = [];

        _store[aggregateId].AddRange(events);

        _projection[aggregateId] = LoadAccount(aggregateId);
    }

    public IEnumerable<Event> GetTransactions(Guid aggregateId)
        => _store.GetValueOrDefault(aggregateId) ?? Enumerable.Empty<Event>();

    public BankAccount LoadAccount(Guid aggregateId)
    {
        var account = new BankAccount(this, aggregateId);
        if (_store.TryGetValue(aggregateId, out var events))
        {
            foreach (var e in events.OrderBy(e => e.Timestamp))
                account.Apply(e);
        }
        return account;
    }

    public BankAccount? GetAccountView(Guid aggregateId)
        => _projection.GetValueOrDefault(aggregateId);
}
