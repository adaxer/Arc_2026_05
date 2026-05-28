using System.Collections;

namespace LabEventSourcing.Events;

public class BankAccount : IEnumerable<Event>
{
    private readonly EventStore _store;
    private readonly Guid _accountId;
    private decimal _balance;
    private bool _isClosed;

    public Guid AccountId => _accountId;

    public BankAccount(EventStore store, Guid accountId)
    {
        _store = store;
        _accountId = accountId;
    }

    public void PrintBalance()
    {
        Console.WriteLine($"Aktueller Kontostand: {_balance} €");
    }

    private void ApplyAndSave(Event @event)
    {
        Apply(@event);
        _store.Save(_accountId, @event);
    }

    public void Apply(Event @event)
    {
        switch (@event)
        {
            case AccountOpenedEvent e:
                _balance = e.InitialBalance;
                break;
            case MoneyDepositedEvent e:
                _balance += e.Amount;
                break;
            case MoneyWithdrawnEvent e:
                _balance -= e.Amount;
                break;
            case AccountClosedEvent:
                _isClosed = true;
                break;
        }
    }

    public void Open(decimal initialBalance)
    {
        if (initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative");

        ApplyAndSave(new AccountOpenedEvent(_accountId, initialBalance));
    }

    public void Deposit(decimal amount)
    {
        if (_isClosed) throw new InvalidOperationException("Account closed");
        if (amount <= 0) throw new ArgumentException("Invalid amount");

        ApplyAndSave(new MoneyDepositedEvent(_accountId, amount));
        PrintBalance();
    }

    public void Withdraw(decimal amount)
    {
        if (_isClosed) throw new InvalidOperationException("Account closed");
        if (amount <= 0) throw new ArgumentException("Invalid amount");
        if (amount > _balance) throw new InvalidOperationException("Insufficient funds");

        ApplyAndSave(new MoneyWithdrawnEvent(_accountId, amount));
        PrintBalance();
    }

    public void Close()
    {
        ApplyAndSave(new AccountClosedEvent(_accountId));
    }
    public decimal GetBalance() =>
        this.Aggregate(0m, (balance, e) => e switch
        {
            AccountOpenedEvent o => o.InitialBalance,
            MoneyDepositedEvent d => balance + d.Amount,
            MoneyWithdrawnEvent w => balance - w.Amount,
            _ => balance
        });

    #region IEnumerable

    public IEnumerator<Event> GetEnumerator()
        => _store.GetTransactions(_accountId).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion
}