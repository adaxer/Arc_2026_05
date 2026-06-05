using System.Collections.ObjectModel;
using ADaxer.MvvmNav.Abstractions.Dialogs;
using ADaxer.MvvmNav.Abstractions.Navigation;
using ADaxer.MvvmNav.Core.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Sparkasse.Client.Common.Messages;

namespace Sparkasse.Client.Common.ViewModels;

/// <summary>
/// Shell view model. Hosts the current module and the current dialog and exposes
/// the navigation entries shown in the side bar.
/// Listens to UserLoggedInMessage and UserLoggedOutMessage to update the window title.
/// </summary>
public partial class ShellViewModel : ViewModelBase, IShellViewModel
{
    private readonly INavigationService _navigation;
    private readonly IMessenger _messenger;
    private string? _currentUser;

    public ShellViewModel(INavigationService navigation, IMessenger messenger)
    {
        _navigation = navigation;
        _messenger = messenger;
        navigation.NavigationStateChanged += NavigationStateChanged;

        // Register for authentication messages
        _messenger.Register<UserLoggedInMessage>(this, (r, m) => OnUserLoggedIn(m));
        _messenger.Register<UserLoggedOutMessage>(this, (r, m) => OnUserLoggedOut());

        NavigationItems =
        [
            new NavigationItem("Welcome","Willkommen", NavigateHomeCommand),
            new NavigationItem("Login", "Anmelden", ShowLoginCommand),
            new NavigationItem("Wetter", "Wettervorhersage", NavigateWeatherCommand),
            new NavigationItem("Todos", "TodoItems", NavigateTodosCommand),
            new NavigationItem("Bank", "Banking", NavigateBankCommand)
        ];
    }

    private void OnUserLoggedIn(UserLoggedInMessage message)
    {
        _currentUser = message.Username;
        Title = GetTitle();
    }

    private void OnUserLoggedOut()
    {
        _currentUser = null;
        Title = GetTitle();
    }

    private void NavigationStateChanged(object? sender, EventArgs e)
    {
        (GoBackCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
        Title = GetTitle();
    }

    private string GetTitle()
    {
        var baseTitle = "Sparkasse";

        if (!string.IsNullOrEmpty(_currentUser))
        {
            baseTitle += $" - {_currentUser}";
        }

        if (CurrentModule is null)
        {
            return baseTitle;
        }

        var currentType = CurrentModule.GetType();
        var moduleTitle = (CurrentModule as ViewModelBase)?.Title ??
            currentType.GetProperty("Title")?.GetValue(CurrentModule)?.ToString() ??
            currentType.Name;

        return $"{baseTitle} - {moduleTitle}";
    }

    [ObservableProperty]
    private object? _currentModule;

    [ObservableProperty]
    private object? _currentDialog;

    [ObservableProperty]
    private object? _title;

    public ObservableCollection<NavigationItem> NavigationItems { get; }

    [RelayCommand]
    private Task NavigateHome()
        => _navigation.NavigateAsync<WelcomeViewModel>();

    [RelayCommand]
    private Task ShowLogin()
        => _navigation.ShowDialogAsync<LoginViewModel>();

    [RelayCommand]
    private Task NavigateWeather()
        => _navigation.NavigateAsync<WeatherViewModel>();

    [RelayCommand]
    private Task NavigateTodos()
        => _navigation.NavigateAsync<TodosViewModel>();

    [RelayCommand]
    private Task NavigateBank()
        => _navigation.NavigateAsync<BankViewModel>();

    [RelayCommand(CanExecute = nameof(CanGoBack))]
    private Task GoBack()
        => _navigation.GoBackAsync();

    private bool CanGoBack()
        => _navigation.CanGoBack();
}
