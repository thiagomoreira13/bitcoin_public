using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.Globalization;

namespace Vinit.Common
{
    public class ObjectUtils
    {
        private static CultureInfo GetCultureInfo()
        {
            return CultureInfo.CreateSpecificCulture("pt-BR");
        }

        public static System.Double ToDouble(System.Object AValue)
        {
            if ((AValue == null) || (AValue == DBNull.Value))
            {
                return 0;
            }
            else
            {
                return Convert.ToDouble(AValue, ObjectUtils.GetCultureInfo());
            }
        }

        public static System.Int32 ToInt(System.Object AValue)
        {

            if (AValue != null )
            {
                if (IntUtils.IsValid(Convert.ToString(AValue)))
                {
                    return Convert.ToInt32(AValue);
                }
                else
                {
                    return 0;
                }
            }
            else
                return 0;
        }

        public static System.Int32 ToIntColaboradores(System.Object AValue)
        {

            if (AValue != null)
            {
                if (IntUtils.IsValid(Convert.ToString(AValue)))
                {
                    return Convert.ToInt32(AValue);
                }
                else
                {
                    return -1;
                }
            }
            else
                return -1;
        }

        public static System.Int64 ToInt64(System.Object AValue)
        {
            if (AValue != null)
            {
                if (IntUtils.IsValid64(Convert.ToString(AValue)))
                {
                    return Convert.ToInt64(AValue);
                }
                else
                {
                    return 0;
                }
            }
            else
                return 0;
        }

        public static System.String ToString(System.Object AValue)
        {
            if (AValue != null)
            {
                return Convert.ToString(AValue);
            }
            else
            {
                return System.String.Empty;
            }
        }

        public static System.String ToStringMesAno(System.Object AValue)
        {
            if (AValue != null)
            {
                DateTime data = ObjectUtils.ToDate(AValue);
                String mesAtual = "Janeiro";
                if (data.Month == 2)
                {
                    mesAtual = "Fevereiro";
                }
                else if (data.Month == 3)
                {
                    mesAtual = "Março";
                }
                else if (data.Month == 4)
                {
                    mesAtual = "Abril";
                }
                else if (data.Month == 5)
                {
                    mesAtual = "Maio";
                }
                else if (data.Month == 6)
                {
                    mesAtual = "Junho";
                }
                else if (data.Month == 7)
                {
                    mesAtual = "Julho";
                }
                else if (data.Month == 8)
                {
                    mesAtual = "Agosto";
                }
                else if (data.Month == 9)
                {
                    mesAtual = "Setembro";
                }
                else if (data.Month == 10)
                {
                    mesAtual = "Outubro";
                }
                else if (data.Month == 11)
                {
                    mesAtual = "Novembro";
                }
                else if (data.Month == 12)
                {
                    mesAtual = "Dezembro";
                }
                mesAtual = mesAtual + " " + data.Year;

                return mesAtual;
            }
            else
            {
                return System.String.Empty;
            }
        }

        public static Color ToColor(System.Object AValue)
        {
            if (AValue != null)
            {
                return ColorTranslator.FromHtml("#" + AValue);
            }
            else
            {
                return Color.White;
            }
        }

        public static System.Char ToChar(System.Object AValue)
        {
            if (AValue != null)
            {
                return Convert.ToChar(AValue);
            }
            else
            {
                return System.Char.MinValue;
            }
        }

        public static System.Object ToEnum(System.Object Avalue, Enum tipo)
        {
            var tipos = tipo.GetType();
            return tipos.GetField(Avalue.ToString());
        }

        public static System.DateTime ToDate(System.Object AValue)
        {

            if (AValue != null)
            {
                if (AValue is System.String)
                {
                    return StringUtils.ToDate((System.String)(AValue));
                }
                else
                    if (AValue is System.DateTime)
                    {
                        return (System.DateTime)(AValue);
                    }
                    else
                        return System.DateTime.MinValue;
            }
            else
                return System.DateTime.MinValue;
        }

        public static System.Boolean ToBoolean(System.Object AValue)
        {
            try
            {
                if (ObjectUtils.ToInt(AValue) > 0)
                {
                    return true;
                }
                return Convert.ToBoolean(AValue);
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public static System.String ToPontuacaoResumo(System.Object AValue)
        {
            try
            {
                return AValue.ToString().Substring(0, AValue.ToString().Length - 2) + " ";
            }
            catch (Exception)
            {
                return AValue.ToString();
            }

        }

        public static System.Decimal ToDecimal(System.Object AValue)
        {
            try
            {
                String temp = ObjectUtils.ToString(AValue);
                if (temp.Contains(","))
                {
                    temp = temp.Replace(".", "");
                }
                else
                {
                    temp = temp.Replace(".", ",");
                }
                return Convert.ToDecimal(temp, ObjectUtils.GetCultureInfo());
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
}
