using CommunityToolkit.Mvvm.Messaging;
using Sparkasse.Client.Common.Services;
using Sparkasse.Client.Common.ViewModels;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for registering the core services for the sample app.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterCommonServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddTransient<WelcomeViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<WeatherViewModel>();
        services.AddTransient<TodosViewModel>();
        services.AddTransient<BankViewModel>();

        // Register CommunityToolkit Messenger as singleton
        services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

        // Register token storage as singleton (shared across all services)
        services.AddSingleton<ITokenStorageService, TokenStorageService>();

        // Register the authentication handler
        services.AddTransient<AuthenticationDelegatingHandler>();

        // Register services that use IHttpClientFactory
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWeatherService, WeatherService>();
        services.AddScoped<ITodoService, TodoService>();

        return services;
    }
}
