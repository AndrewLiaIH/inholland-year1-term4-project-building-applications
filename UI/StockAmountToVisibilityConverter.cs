using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UI
{
    class StockAmountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int stockAmount && parameter is string color)
            {
                if (color == "Yellow" && stockAmount < 10 && stockAmount > 0)
                {
                    return Visibility.Visible;
                }
                if (color == "Red" && stockAmount == 0)
                {
                    return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
