using System;
using System.Globalization;
using Industria4.Models;
using Xamarin.Forms;

namespace Industria4.Converters
{
    public class ChartIsCorrenteConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Sensor)value != null)
            {
                Sensor sensor = value as Sensor;

                if (sensor != null && sensor.TiposGrandeza.Grandeza.ToUpper().Equals("CORRENTE") )
                {
                    return true;
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}