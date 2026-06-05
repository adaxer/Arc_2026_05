using ADaxer.MvvmNav.Abstractions.Dialogs;
using ADaxer.MvvmNav.Abstractions.Navigation;
using Sparkasse.Client.Common.ViewModels;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for registering the Avalonia specific services for the sample app.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterAvaloniaSpecificServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<ShellViewModel>();
        services.AddSingleton<IShellViewModel>(sp => sp.GetRequiredService<ShellViewModel>());
        services.AddSingleton<IDialogHost>(sp => sp.GetRequiredService<ShellViewModel>());

        return services;
    }
}
