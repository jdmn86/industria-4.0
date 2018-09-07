using System;
using System.Globalization;
using Industria4.Helpers;
using Industria4.Models;
using Xamarin.Forms;

namespace Industria4.Converters
{
    public class ChatToRightConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Message)value != null)
            {
                Message ms = value as Message;

                if (ms.toUser != null && ms.toUser.Equals(Settings.Name))
                {
                    return "End";
                }
            }

            return "Start";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}