using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Industria4.Validations
{
    public class IsBetweenValueOfByteRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
          
            var str = value as string;

            if (str==null ||str.Length==0)
            {
                return true;
            }

            Regex regex = new Regex(@"^([1-9]|[1-9][0-9]|[1-2][0-5][0-5])$");
            Match match = regex.Match(str);

           return match.Success;
        }
    }
}
