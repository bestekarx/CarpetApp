using System.Globalization;

namespace CarpetApp.CustomConverters;

public class StringNotNullOrEmptyToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is string str && !string.IsNullOrEmpty(str);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 