using ADaxer.MvvmNav.Abstractions.Navigation;
using ADaxer.MvvmNav.Core.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Sparkasse.Client.Common.ViewModels;

/// <summary>
/// Welcome module. Implements <see cref="INavigationAware"/> so the module can refresh
/// its state every time it is activated.
/// </summary>
public partial class WelcomeViewModel : ViewModelBase, INavigationAware
{
    [ObservableProperty]
    private string _heading = "Welcome";

    [ObservableProperty]
    private string _message = string.Empty;

    public WelcomeViewModel()
    {
        Title = "Welcome";
    }

    public Task OnNavigatedToAsync(NavigationParameters context)
    {
        Message = $"Willkommen in der Sparkasse ({DateTime.Now:T})";
        return Task.CompletedTask;
    }
}
