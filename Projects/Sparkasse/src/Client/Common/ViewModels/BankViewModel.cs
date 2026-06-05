using System.Collections.ObjectModel;
using ADaxer.MvvmNav.Abstractions.Navigation;
using ADaxer.MvvmNav.Core.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Sparkasse.Client.Common.ViewModels;

public partial class TransactionDto : ObservableObject
{
    [ObservableProperty]
    private DateTime _date;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private decimal _amount;

    [ObservableProperty]
    private decimal _balance;
}

/// <summary>
/// Banking module. Manages bank account operations.
/// </summary>
public partial class BankViewModel : ViewModelBase, INavigationAware
{
    [ObservableProperty]
    private string _accountNumber = string.Empty;

    [ObservableProperty]
    private decimal _balance = 0;

    [ObservableProperty]
    private string _accountHolder = string.Empty;

    [ObservableProperty]
    private decimal _amount = 0;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _hasAccount;

    public ObservableCollection<TransactionDto> Transactions { get; } = [];

    public BankViewModel()
    {
        Title = "Banking";
    }

    public async Task OnNavigatedToAsync(NavigationParameters context)
    {
        await CheckAccount();
    }

    private async Task CheckAccount()
    {
        IsLoading = true;
        try
        {
            // TODO: Call API to check if account exists
            await Task.Delay(300);

            // Simulate existing account
            HasAccount = false;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task CreateAccount()
    {
        if (string.IsNullOrWhiteSpace(AccountHolder))
        {
            StatusMessage = "Bitte Kontoinhaber angeben.";
            return;
        }

        IsLoading = true;
        try
        {
            // TODO: Call API to create account
            await Task.Delay(500);

            AccountNumber = $"DE{Random.Shared.Next(10, 99)} {Random.Shared.Next(1000, 9999)} {Random.Shared.Next(1000, 9999)} {Random.Shared.Next(1000, 9999)} {Random.Shared.Next(1000, 9999)} {Random.Shared.Next(10, 99)}";
            Balance = 0;
            HasAccount = true;
            StatusMessage = "Konto erfolgreich erstellt.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task Deposit()
    {
        if (Amount <= 0)
        {
            StatusMessage = "Bitte einen gültigen Betrag eingeben.";
            return;
        }

        IsLoading = true;
        try
        {
            // TODO: Call API to deposit
            await Task.Delay(300);

            Balance += Amount;
            Transactions.Insert(0, new TransactionDto
            {
                Date = DateTime.Now,
                Description = "Einzahlung",
                Amount = Amount,
                Balance = Balance
            });

            StatusMessage = $"Einzahlung von {Amount:C} erfolgreich.";
            Amount = 0;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task Withdraw()
    {
        if (Amount <= 0)
        {
            StatusMessage = "Bitte einen gültigen Betrag eingeben.";
            return;
        }

        if (Amount > Balance)
        {
            StatusMessage = "Unzureichendes Guthaben.";
            return;
        }

        IsLoading = true;
        try
        {
            // TODO: Call API to withdraw
            await Task.Delay(300);

            Balance -= Amount;
            Transactions.Insert(0, new TransactionDto
            {
                Date = DateTime.Now,
                Description = "Auszahlung",
                Amount = -Amount,
                Balance = Balance
            });

            StatusMessage = $"Auszahlung von {Amount:C} erfolgreich.";
            Amount = 0;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task CloseAccount()
    {
        IsLoading = true;
        try
        {
            // TODO: Call API to close account
            await Task.Delay(500);

            HasAccount = false;
            AccountNumber = string.Empty;
            AccountHolder = string.Empty;
            Balance = 0;
            Transactions.Clear();
            StatusMessage = "Konto wurde geschlossen.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task GetStatement()
    {
        IsLoading = true;
        try
        {
            // TODO: Call API to get statement
            await Task.Delay(300);

            StatusMessage = $"Kontoauszug aktualisiert am {DateTime.Now:dd.MM.yyyy HH:mm}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
