using LabEventSourcing.Events;

var accountId = Guid.Empty;
var store = new EventStore();

Console.OutputEncoding = System.Text.Encoding.UTF8;

while (true)
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

    try
    {
        switch (input)
        {
            case "1":
                accountId = Guid.NewGuid();
                Console.WriteLine($"Konto eroeffnet: {accountId}");

                new BankAccount(store, accountId)
                    .Open(GetAmount("Startguthaben: "));
                break;

            case "2":
                Account(accountId)?
                    .Deposit(GetAmount());
                break;

            case "3":
                Account(accountId)?
                    .Withdraw(GetAmount());
                break;

            case "4":
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
                break;

            case "5":
                Account(accountId)?.Close();
                break;

            default:
                return;
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    BankAccount? Account(Guid accountId)
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