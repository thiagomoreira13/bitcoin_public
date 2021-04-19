using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Vinit.Common
{
    public class ByteUtils
    {
        public static System.String ToCoordinate(byte[] byteArray)
        {
            Int64 temp = BitConverter.ToInt64(byteArray, 0);
            Double res = temp;
            while (res > 180)
            {
                res = res / 10.0;
            }
            if (res > 0)
            {
                res = -res;
            }
            String res2 = res.ToString();
            return res2;
        }

        public static System.String ToString(byte[] byteArray)
        {
            Int64 temp = BitConverter.ToInt64(byteArray, 0);
            String res2 = temp.ToString();
            return res2;
        }
    }
}
