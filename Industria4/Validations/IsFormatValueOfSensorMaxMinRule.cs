using System;
using System.Text.RegularExpressions;

namespace Industria4.Validations
{
    public class IsFormatValueOfSensorMaxMinRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {

            string d;

            if (typeof(T) == typeof(double))
            {
                d = Convert.ToString(value);

                if (d == null || d.Length == 0)
                {
                    return true;

                }else{
                    Regex regex = new Regex(@"^\-{0,1}\d+(.\d{2}){0,1}$");
                    Match match = regex.Match(d);

                    return match.Success;
                }
            }

            return false;
        }
    }
}