using System;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Industria4.Converters
{
    public class TextToByteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
           // byte aux =(byte) value;

         //   string aux2 =  int.Parse((byte)aux); 

            // throw new NotImplementedException("TextToByteConverter converterBack");
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string aux=value as string;

            byte aux2= byte.Parse((string)value);

            // throw new NotImplementedException("TextToByteConverter converterBack");
            return aux2;
        }
    }
}
