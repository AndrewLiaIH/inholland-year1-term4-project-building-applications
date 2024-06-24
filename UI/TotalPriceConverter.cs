using System.Globalization;
using System.Windows.Data;

namespace UI
{
    class TotalPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal totalPrice)
            {
                // Format the price to have a comma instead of a dot for decimal separator
                string formattedPrice = totalPrice.ToString("0.00", CultureInfo.InvariantCulture);
                formattedPrice = formattedPrice.Replace('.', ',');

                return formattedPrice;
            }

            return value; // Return the original value if it's not a decimal
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
