using System.Collections;

namespace Sparkasse.Domain.Aggregates;

public class BankAccount : BaseEntity
{
    private decimal _balance;
    private bool _isClosed;
    private readonly Guid _accountId = Guid.Empty;

    public BankAccount(Guid accountId)
    {
        _accountId = accountId;
    }

    public void PrintBalance()
    {
        Console.WriteLine($"Aktueller Kontostand: {_balance} €");
    }

    public void Open(decimal initialBalance)
    {
        if (initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative");

        var @event = new AccountOpenedEvent(_accountId, initialBalance);
        Apply(@event);
        AddDomainEvent(@event);
    }

    public void Deposit(decimal amount)
    {
        if (_isClosed) throw new InvalidOperationException("Account closed");
        if (amount <= 0) throw new ArgumentException("Invalid amount");

        var @event = new MoneyDepositedEvent(_accountId, amount);
        Apply(@event);
        AddDomainEvent(@event);
    }

    public void Withdraw(decimal amount)
    {
        if (_isClosed) throw new InvalidOperationException("Account closed");
        if (amount <= 0) throw new ArgumentException("Invalid amount");
        if (amount > _balance) throw new InvalidOperationException("Insufficient funds");

        var @event = new MoneyWithdrawnEvent(_accountId, amount);
        Apply(@event);
        AddDomainEvent(@event);
    }

    public void Close()
    {
        var @event = new AccountClosedEvent(_accountId);
        Apply(@event);
        AddDomainEvent(@event);
    }

    public void Apply(BaseEvent @event)
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
}
