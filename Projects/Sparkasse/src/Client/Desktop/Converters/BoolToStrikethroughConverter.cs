using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Sparkasse.Client.Desktop.Converters;

public class BoolToStrikethroughConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isDone && isDone)
        {
            return Avalonia.Media.TextDecorations.Strikethrough;
        }
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
