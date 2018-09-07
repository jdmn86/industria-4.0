using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;

namespace Industria4.Converters
{
    public class ListIsEmptyHideConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(((IList)value)==null){
                return false;
            }else if(((IList)value).Count > 0){
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}