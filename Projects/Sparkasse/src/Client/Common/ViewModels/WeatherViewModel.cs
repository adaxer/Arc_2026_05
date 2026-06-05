using System.Collections.ObjectModel;
using ADaxer.MvvmNav.Abstractions.Navigation;
using ADaxer.MvvmNav.Core.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sparkasse.Client.Common.Services;

namespace Sparkasse.Client.Common.ViewModels;

/// <summary>
/// Weather forecast module. Displays weather information from the API.
/// </summary>
public partial class WeatherViewModel : ViewModelBase, INavigationAware
{
    private readonly IWeatherService _weatherService;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public ObservableCollection<WeatherForecastDto> Forecasts { get; } = [];

    public WeatherViewModel(IWeatherService weatherService)
    {
        _weatherService = weatherService;
        Title = "Wettervorhersage";
    }

    public Task OnNavigatedToAsync(NavigationParameters context)
    {
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task GetWeather()
    {
        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            var forecasts = await _weatherService.GetWeatherForecastsAsync();

            Forecasts.Clear();
            foreach (var forecast in forecasts)
            {
                Forecasts.Add(forecast);
            }

            if (!Forecasts.Any())
            {
                ErrorMessage = "Keine Wetterdaten verfügbar.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Fehler beim Abrufen der Wetterdaten: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
