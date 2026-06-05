using System.Windows.Input;

namespace Sparkasse.Client.Common.ViewModels;

/// <summary>
/// A simple way to combine a Name of a "Page" to show and a Command to navigate to it.
/// MvvmNav could offer something like this, but we leave it up to you (To add keyboard shortcuts, or icons, or navigation parameters).
/// </summary>
/// <param name="Title">The title of the navigation item.</param>
/// <param name="Command">The command to execute when the navigation item is selected.</param>
public sealed record NavigationItem(string Title, string Subtitle, ICommand Command);
