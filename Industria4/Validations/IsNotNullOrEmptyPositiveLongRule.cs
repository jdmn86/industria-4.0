using System;
using System.Text.RegularExpressions;

namespace Industria4.Validations
{
    public class IsNotNullOrEmptyPositiveLongRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            string i;

            if (typeof(T) == typeof(long))
            {
                i = Convert.ToString(value);

                if (i == null || i.Length == 0)
                {
                    return false;
                }
                else
                {
                    long maxLong = long.MaxValue;
                    long v = Convert.ToInt64(value);
                    if(v>0 && v< maxLong){
                        Regex regex = new Regex(@"^\-{0}\d+(\.){0}$");
                        Match match = regex.Match(i);

                        return match.Success;
                    }
                    return false;
                }

            }


            return false;
        }
    }
}