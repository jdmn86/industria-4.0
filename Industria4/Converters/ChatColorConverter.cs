using System;
using System.Globalization;
using Industria4.Helpers;
using Industria4.Models;
using Xamarin.Forms;

namespace Industria4.Converters
{
    public class ChatColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Message)value != null)
            {
                Message ms = value as Message;

                if(ms.toUser !=null && ms.toUser.Equals(Settings.Name)){
                    return Color.Green;
                }
            }

            return Color.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}