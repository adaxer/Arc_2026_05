using System.Collections;

namespace EventSourcingSolution;

internal class Program
{
    private static Guid accountId = Guid.Empty;
    private static EventStore store = new();

    static void Main(string[] args)
    {
        bool isDone = false;
        while (!isDone)
        {
            Console.WriteLine(@"
                1. Konto eröffnen
                2. Einzahlen
                3. Abheben
                4. Kontoauszug
                5. Konto schließen 
                0. Beenden");
            var input = Console.ReadLine();
            Console.Clear();

            Action toDo = input switch
            {
                "1" => OpenAccount,
                "2" => Deposit,
                "3" => Withdraw,
                "4" => Balance,
                "5" => CloseAccount,
                "0" => () => isDone = true,
                _ => () => Console.WriteLine("Wie bitte?")
            };
            try
            {
                toDo.Invoke();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

        }
    }

    private static void CloseAccount()
    {
        Account(accountId)?.Close();
        Balance();
    }

    private static void Balance()
    {
        var account = Account(accountId);
        if (account != null)
        {
            account.PrintBalance();
            Console.WriteLine("Auszug:");
            foreach (var e in account)
            {
                Console.WriteLine(e switch
                {
                    AccountOpenedEvent o => $"{o.Timestamp:g} | +{o.InitialBalance,4} € \tKonto eröffnet",
                    MoneyDepositedEvent d => $"{d.Timestamp:g} | +{d.Amount,4} € \tEinzahlung",
                    MoneyWithdrawnEvent w => $"{w.Timestamp:g} | -{w.Amount,4} € \tAbhebung",
                    AccountClosedEvent c => $"{c.Timestamp:g} |        \tKonto geschlossen",
                    _ => "Unbekanntes Event"
                });
            }
        }
    }

    private static void Withdraw()
    {
        Account(accountId)?.Withdraw(GetAmount("Abhebungsbetrag: "));
    }

    private static void Deposit()
    {
        Account(accountId)?.Deposit(GetAmount("Einzahlungsbetrag: "));
    }

    private static void OpenAccount()
    {
        accountId = Guid.NewGuid();
        new BankAccount(store, accountId)?.Open(GetAmount("Startguthaben: "));
    }

    static BankAccount? Account(Guid accountId)
    {
        var account = store.GetAccountView(accountId);
        return account ?? throw new KeyNotFoundException("Konto nicht gefunden!");
    }

    static decimal GetAmount(string message = "Betrag: ")
    {
        Console.Write(message);
        return decimal.Parse(Console.ReadLine()!);
    }
}

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




