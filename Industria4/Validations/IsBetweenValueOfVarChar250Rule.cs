using System;
using System.Text.RegularExpressions;

namespace Industria4.Validations
{
    public class IsBetweenValueOfVarChar250Rule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            var str = value as string;

            if (str==null || str.Length == 0)
            {
                return true;
            }
           
            if (str.Length > 250 ){
                return false;
            }
            
            return true;
        }
    }
}
