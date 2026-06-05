using ADaxer.MvvmNav.Abstractions.Navigation;
using Avalonia.Controls;

namespace Sparkasse.Client.Desktop.Views;

public partial class ShellWindow : Window, IAvaloniaShellView
{
    public ShellWindow()
    {
        InitializeComponent();
    }
}
