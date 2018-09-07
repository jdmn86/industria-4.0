using System;
using System.Globalization;
using Xamarin.Forms;

namespace Industria4.Converters
{
    public class EditOrAddExpandeTwoConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
            {
                return 2;
            }
            else
            {
                return 1;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}