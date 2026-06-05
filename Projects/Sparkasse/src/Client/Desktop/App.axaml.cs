using System;
using ADaxer.MvvmNav.Avalonia.Hosting;
using Avalonia;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace Sparkasse.Client.Desktop;

public partial class App : Application
{
    public IServiceProvider? ServiceProvider { get; set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        ArgumentNullException.ThrowIfNull(ServiceProvider);

        var starter = ServiceProvider.GetRequiredService<IMvvmNavStarter>();
        starter.Initialize(this);
        await starter.StartAsync();

        base.OnFrameworkInitializationCompleted();
    }
}

