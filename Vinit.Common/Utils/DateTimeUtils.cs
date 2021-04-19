using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Vinit.Common
{
    public class DateTimeUtils
    {

        public static System.Boolean IsValidTime(System.String AValue)
        {
            System.DateTime dateValid;
            return (System.String.IsNullOrEmpty(AValue) || System.DateTime.TryParseExact(AValue, "HH:mm:ss", new CultureInfo("pt-BR"), DateTimeStyles.None, out dateValid));
        }





    }
}
