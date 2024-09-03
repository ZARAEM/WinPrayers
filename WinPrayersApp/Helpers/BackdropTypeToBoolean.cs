using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace WinPrayersApp.Helpers;

public class BackdropTypeToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (parameter is string enumString && value != null)
        {
            if (!Enum.IsDefined(value.GetType(), value))
            {
                throw new ArgumentException("Value must be a defined enum value.");
            }

            var enumType = value.GetType();
            var enumValue = Enum.Parse(enumType, enumString);

            return enumValue.Equals(value);
        }

        throw new ArgumentException("Parameter must be a string representing an enum value.");
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is bool boolValue && boolValue && parameter is string enumString)
        {
            return Enum.Parse(targetType, enumString);
        }

        return DependencyProperty.UnsetValue;
    }
}
