using Sparkasse.Application.Common.Models;
using Sparkasse.Domain.Aggregates;
using Sparkasse.Domain.Events;

namespace Sparkasse.Application.BankAccounts;

public class BankAccountService : IBankAccountService
{
    private readonly EventStore _eventStore;
    private readonly IMediator _mediator;

    public BankAccountService(EventStore eventStore, IMediator mediator)
    {
        _eventStore = eventStore;
        _mediator = mediator;
    }

    public async Task<Result<Guid>> OpenAccountAsync(decimal initialBalance, CancellationToken cancellationToken = default)
    {
        var accountId = Guid.NewGuid();
        var account = new BankAccount(accountId);
        account.Open(initialBalance);
        await SaveAndFireEventsAsync(accountId, account, cancellationToken);
        return Result<Guid>.Success(accountId);
    }

    public async Task<Result<decimal>> CloseAccountAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var account = _eventStore.LoadAccount(accountId);

        var balance = CalculateBalance(accountId);
        account.Close();
        await SaveAndFireEventsAsync(accountId, account, cancellationToken);

        return Result<decimal>.Success(balance);
    }

    public async Task<Result<decimal>> DepositAsync(Guid accountId, decimal amount, CancellationToken cancellationToken = default)
    {
        var account = _eventStore.LoadAccount(accountId);

        account.Deposit(amount);
        await SaveAndFireEventsAsync(accountId, account, cancellationToken);

        var newBalance = CalculateBalance(accountId);
        return Result<decimal>.Success(newBalance);
    }

    public async Task<Result<decimal>> WithdrawAsync(Guid accountId, decimal amount, CancellationToken cancellationToken = default)
    {
        var account = _eventStore.LoadAccount(accountId);

        account.Withdraw(amount);
        await SaveAndFireEventsAsync(accountId, account, cancellationToken);

        var newBalance = CalculateBalance(accountId);
        return Result<decimal>.Success(newBalance);
    }

    public Task<Result<decimal>> GetBalanceAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var balance = CalculateBalance(accountId);
        return Task.FromResult(Result<decimal>.Success(balance));
    }

    public Task<Result<IEnumerable<Queries.BankAccountValue>>> GetStatementAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var transactions = _eventStore.GetTransactions(accountId);
        var statement = transactions.AsEnumerable();

        return Task.FromResult(Result<IEnumerable<Queries.BankAccountValue>>.Success(statement));
    }

    private async Task SaveAndFireEventsAsync(Guid accountId, BankAccount account, CancellationToken cancellationToken)
    {
        var events = account.DomainEvents
            .OfType<Domain.Events.BankAccountEvent>()
            .Select(ConvertToQueryEvent)
            .ToArray();

        if (events.Length > 0)
        {
            _eventStore.Save(accountId, events);
        }

        foreach (var @event in account.DomainEvents)
        {
            await _mediator.Publish(@event, cancellationToken);
        }
    }

    private decimal CalculateBalance(Guid accountId)
    {
        var transactions = _eventStore.GetTransactions(accountId);
        var balance = 0m;

        foreach (var @event in transactions)
        {
            balance += GetAmountFromEvent(@event);
        }

        return balance;
    }

    private static Queries.BankAccountValue ConvertToQueryEvent(Domain.Events.BankAccountEvent domainEvent)
    {
        return domainEvent switch
        {
            AccountOpenedEvent e => new Queries.BankAccountValue(e.Timestamp, e.InitialBalance),
            MoneyDepositedEvent e => new Queries.BankAccountValue(e.Timestamp, e.Amount),
            MoneyWithdrawnEvent e => new Queries.BankAccountValue(e.Timestamp, -e.Amount),
            AccountClosedEvent e => new Queries.BankAccountValue(e.Timestamp, null),
            _ => throw new ArgumentException($"Unknown event type: {domainEvent.GetType().Name}")
        };
    }

    private static decimal GetAmountFromEvent(Queries.BankAccountValue @event)
    {
        return @event.Amount ?? 0m;
    }
}
