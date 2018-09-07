using System;
namespace Industria4.Validations
{
    public class IsNotNullOrEmptyDoubleRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            string d;
            if(typeof(T) == typeof(double)){
                d = Convert.ToString(value);

                if (d == null || d.Length == 0)
                {
                    return false;
                }
            }
            //  double d = double.Parse() ;//( value as double);
       //     string str = value as double;

            return true;
        }
    }
}
