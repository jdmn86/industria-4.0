using System;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms;

namespace Industria4.Converters
{
    public class WebNavigatingEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.WriteLine("WebNavigatingEventArgsConverter");
            var eventArgs = value as WebNavigatingEventArgs;
            if (eventArgs == null)
                throw new ArgumentException("Expected WebNavigatingEventArgs as value", "value");

            return eventArgs.Url;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
