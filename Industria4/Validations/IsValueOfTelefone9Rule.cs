using System;
namespace Industria4.Validations
{
    public class IsValueOfTelefone9Rule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            var str = value as string;

            if (str==null ||str.Length == 0)
            {
                return true;
            }

            if (str.Length != 9 )
            {
                return false;
            }

            return true;
        }
    }
}