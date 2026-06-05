using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace Sparkasse.Client.Desktop.Converters;

public class AmountToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is decimal amount)
        {
            return amount >= 0 ? Brushes.Green : Brushes.Red;
        }
        return Brushes.Black;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
