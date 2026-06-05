using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sparkasse.Client.Common.Services;
using Sparkasse.Client.Common.ViewModels;
using Sparkasse.Client.Desktop.Views;
using Sparkasse.Shared;

namespace Sparkasse.Client.Desktop;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.AddServiceDefaults();
        builder.Services.AddServiceDiscovery();

        // Register named HttpClient with Service Discovery for WebApi
        // The AuthenticationDelegatingHandler is added to automatically attach Bearer tokens
        // Usage in services: IHttpClientFactory.CreateClient("WebApi")
        builder.Services.AddHttpClient("WebApi", client =>
        {
            client.BaseAddress = new Uri($"https+http://{Services.WebApi}");
        })
        .AddHttpMessageHandler<AuthenticationDelegatingHandler>()
        .AddServiceDiscovery();

        builder.Services
            .RegisterCommonServices()
            .RegisterAvaloniaSpecificServices();

        builder.Services.AddMvvmNav()
            .WithShell<ShellWindow, ShellViewModel>()
            .WithStartupNavigation<WelcomeViewModel>();

        var host = builder.Build();

        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools()
#endif
            .WithInterFont()
            .LogToTrace()
            .AfterSetup(_ =>
            {
                if (Application.Current is App app)
                {
                    app.ServiceProvider = host.Services;
                }
            });
    }
}
