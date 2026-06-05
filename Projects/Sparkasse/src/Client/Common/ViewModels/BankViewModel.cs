using System.Collections.ObjectModel;
using ADaxer.MvvmNav.Abstractions.Navigation;
using ADaxer.MvvmNav.Core.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sparkasse.Client.Common.Services;

namespace Sparkasse.Client.Common.ViewModels;

public partial class TransactionViewModel : ObservableObject
{
    [ObservableProperty]
    private DateTime _date;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private decimal _amount;

    [ObservableProperty]
    private decimal _balance;

    public static TransactionViewModel FromDto(BankAccountTransactionDto dto)
    {
        return new TransactionViewModel
        {
            Date = dto.Date,
            Description = dto.Description,
            Amount = dto.Amount,
            Balance = dto.Balance
        };
    }
}

/// <summary>
/// Banking module. Manages bank account operations via WebApi.
/// </summary>
public partial class BankViewModel : ViewModelBase, INavigationAware
{
    private readonly IBankAccountService _bankAccountService;
    private Guid? _accountId;

    [ObservableProperty]
    private string _accountNumber = string.Empty;

    [ObservableProperty]
    private decimal _balance = 0;

    [ObservableProperty]
    private string _accountHolder = string.Empty;

    [ObservableProperty]
    private decimal _amount = 0;

    [ObservableProperty]
    private decimal _initialBalance = 100;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _hasAccount;

    public ObservableCollection<TransactionViewModel> Transactions { get; } = [];

    public BankViewModel(IBankAccountService bankAccountService)
    {
        _bankAccountService = bankAccountService;
        Title = "Banking";
    }

    public async Task OnNavigatedToAsync(NavigationParameters context)
    {
        await CheckAccount();
    }

    private async Task CheckAccount()
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            // Check if we have a stored account ID from previous session
            _accountId = _bankAccountService.CurrentAccountId;

            if (_accountId.HasValue)
            {
                // Try to get balance to verify account still exists
                Balance = await _bankAccountService.GetBalanceAsync(_accountId.Value);
                AccountNumber = _accountId.Value.ToString();
                HasAccount = true;
                StatusMessage = "Konto geladen.";
            }
            else
            {
                HasAccount = false;
            }
        }
        catch (Exception ex)
        {
            // Account might not exist anymore (in-memory store was reset)
            _accountId = null;
            HasAccount = false;
            ErrorMessage = $"Kontoprüfung fehlgeschlagen: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task CreateAccount()
    {
        if (InitialBalance < 0)
        {
            ErrorMessage = "Anfangssaldo darf nicht negativ sein.";
            return;
        }

        IsLoading = true;
        ErrorMessage = null;
        StatusMessage = string.Empty;
        try
        {
            _accountId = await _bankAccountService.OpenAccountAsync(InitialBalance);
            AccountNumber = _accountId.Value.ToString();
            Balance = InitialBalance;
            HasAccount = true;
            StatusMessage = $"Konto erfolgreich erstellt. ID: {_accountId}";

            // Load initial statement
            await LoadStatement();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Fehler beim Erstellen des Kontos: {ex.Message}";
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
            ErrorMessage = "Bitte einen gültigen Betrag eingeben (> 0).";
            return;
        }

        if (!_accountId.HasValue)
        {
            ErrorMessage = "Kein Konto vorhanden.";
            return;
        }

        IsLoading = true;
        ErrorMessage = null;
        StatusMessage = string.Empty;
        try
        {
            Balance = await _bankAccountService.DepositAsync(_accountId.Value, Amount);
            StatusMessage = $"Einzahlung von {Amount:C} erfolgreich. Neuer Kontostand: {Balance:C}";
            Amount = 0;

            // Reload statement to show new transaction
            await LoadStatement();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Fehler bei der Einzahlung: {ex.Message}";
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
            ErrorMessage = "Bitte einen gültigen Betrag eingeben (> 0).";
            return;
        }

        if (!_accountId.HasValue)
        {
            ErrorMessage = "Kein Konto vorhanden.";
            return;
        }

        IsLoading = true;
        ErrorMessage = null;
        StatusMessage = string.Empty;
        try
        {
            Balance = await _bankAccountService.WithdrawAsync(_accountId.Value, Amount);
            StatusMessage = $"Auszahlung von {Amount:C} erfolgreich. Neuer Kontostand: {Balance:C}";
            Amount = 0;

            // Reload statement to show new transaction
            await LoadStatement();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Fehler bei der Auszahlung: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task CloseAccount()
    {
        if (!_accountId.HasValue)
        {
            ErrorMessage = "Kein Konto vorhanden.";
            return;
        }

        IsLoading = true;
        ErrorMessage = null;
        StatusMessage = string.Empty;
        try
        {
            var finalBalance = await _bankAccountService.CloseAccountAsync(_accountId.Value);

            HasAccount = false;
            _accountId = null;
            AccountNumber = string.Empty;
            AccountHolder = string.Empty;
            Balance = 0;
            Transactions.Clear();
            StatusMessage = $"Konto wurde geschlossen. Finaler Kontostand: {finalBalance:C}";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Fehler beim Schließen des Kontos: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task GetStatement()
    {
        if (!_accountId.HasValue)
        {
            ErrorMessage = "Kein Konto vorhanden.";
            return;
        }

        await LoadStatement();
    }

    private async Task LoadStatement()
    {
        if (!_accountId.HasValue) return;

        IsLoading = true;
        ErrorMessage = null;
        try
        {
            var statement = await _bankAccountService.GetStatementAsync(_accountId.Value);

            Transactions.Clear();
            foreach (var transaction in statement.OrderByDescending(t => t.Date))
            {
                Transactions.Add(TransactionViewModel.FromDto(transaction));
            }

            StatusMessage = $"Kontoauszug aktualisiert am {DateTime.Now:dd.MM.yyyy HH:mm}";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Fehler beim Laden des Kontoauszugs: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
