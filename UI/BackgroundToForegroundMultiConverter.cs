using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace UI
{
    public class BackgroundToForegroundMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is SolidColorBrush backgroundBrush && values[1] is SolidColorBrush lightBrush && values[2] is SolidColorBrush darkBrush)
            {
                double brightness = (backgroundBrush.Color.R * 0.299 + backgroundBrush.Color.G * 0.587 + backgroundBrush.Color.B * 0.114) / 255;
                return brightness > 0.5 ? darkBrush : lightBrush;
            }

            return Brushes.Black;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
