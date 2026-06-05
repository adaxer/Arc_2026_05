using ADaxer.MvvmNav.Abstractions.Dialogs;
using ADaxer.MvvmNav.Abstractions.Navigation;
using ADaxer.MvvmNav.Core.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sparkasse.Client.Common.Services;

namespace Sparkasse.Client.Common.ViewModels;

/// <summary>
/// Login dialog. Implements <see cref="IDialogAware"/> to handle dialog interactions.
/// </summary>
public partial class LoginViewModel : DialogViewModelBase, IDialogExchange
{
    private readonly IUserService _userService;

    [ObservableProperty]
    private string _username = "administrator@localhost";

    [ObservableProperty]
    private string _password = "Administrator1!";

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isLoading;

    public DialogExchangeInfo DialogExchange => new DialogExchangeInfo([
            new DialogCommandInfo("OK", DialogResult.True),
            new DialogCommandInfo("Cancel", DialogResult.None)])
    { ContinueAsync = DialogDoneAsync };

    private async Task<bool> DialogDoneAsync(DialogResult result, CancellationToken token)
    {
        if (result == DialogResult.None)
        {
            return true;
        }
        var isLoggedIn = await LoginAsync();
        return isLoggedIn;
    }

    public LoginViewModel(IUserService userService)
    {
        _userService = userService;
        Title = "Anmelden";
    }

    private async Task<bool> LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Benutzername und Passwort sind erforderlich.";
            return false;
        }

        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            var result = await _userService.LoginAsync(Username, Password);

            if (result.Success)
            {
                return true;
            }
            else
            {
                ErrorMessage = result.ErrorMessage ?? "Login fehlgeschlagen.";
                return false;
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}
