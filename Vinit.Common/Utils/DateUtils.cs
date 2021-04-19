using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Vinit.Common
{
    public class DateUtils
    {
        private static CultureInfo GetCultureInfo()
        {
            return CultureInfo.CreateSpecificCulture("pt-BR");
        }

        public static System.Boolean IsValid(System.String AValue)
        {
            System.DateTime dateValid;

            return (System.String.IsNullOrEmpty(AValue) || (DateTime.TryParseExact(AValue, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dateValid)));
        }
        public static System.Boolean IsValidComplete(System.String AValue)
        {
            System.DateTime dateValid;

            return (System.String.IsNullOrEmpty(AValue) || (DateTime.TryParseExact(AValue, "dd/MM/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out dateValid)));
        }

        public static System.DateTime ToDateTime(System.String AValue)
        {
            System.DateTime dateValid;

            DateTime.TryParseExact(AValue, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dateValid);

            return dateValid;
        }

        public static System.Boolean IsValid(System.DateTime AValue)
        {
            return DateUtils.IsValid(Convert.ToString(AValue));
        }

        public static System.Boolean IsMinValue(System.DateTime AValue)
        {
            return DateTime.MinValue.Equals(AValue);
        }

        public static System.Object ToDateObject(System.DateTime AValue)
        {

            if (AValue != null && AValue.Year > 1801 && AValue.Year < 2240)
            {
                return AValue;
            }
            else
                return DBNull.Value;
        }

        public static System.String ToString(System.DateTime AValue)
        {
            if (DateUtils.IsMinValue(AValue) || AValue.Year <= 1800)
            {
                return System.String.Empty;
            }
            else
            {
                if (AValue.Year > 2200)
                {
                    return "";
                }

                return AValue.ToString("dd/MM/yyyy");
            }
        }

        public static System.String ToStringDiaMes(System.DateTime AValue)
        {
            if (DateUtils.IsMinValue(AValue) || AValue.Year <= 1800)
            {
                return System.String.Empty;
            }
            else
            {
                if (AValue.Year > 2200)
                {
                    return "";
                }

                return AValue.ToString("dd/MM");
            }
        }

        public static System.String ToStringMesAnoSimples(System.DateTime AValue)
        {
            if (DateUtils.IsMinValue(AValue) || AValue.Year <= 1800)
            {
                return System.String.Empty;
            }
            else
            {
                if (AValue.Year > 2200)
                {
                    return "";
                }

                return AValue.ToString("MM/yyyy");
            }
        }

        public static System.String ToStringSimples(System.DateTime AValue)
        {
            if (DateUtils.IsMinValue(AValue) || AValue.Year <= 1800)
            {
                return System.String.Empty;
            }
            else
            {
                if (AValue.Year > 2200)
                {
                    return "";
                }

                return AValue.ToString("ddMMyy");
            }
        }

        public static System.String ToStringValidacao(System.DateTime AValue)
        {
            if (DateUtils.IsMinValue(AValue) || AValue.Year < 1901)
            {
                return System.String.Empty;
            }
            else
            {
                return AValue.ToString("dd/MM/yyyy");
            }
        }

        public static System.String ToStringCompleto(System.DateTime AValue)
        {
            if (DateUtils.IsMinValue(AValue) || AValue.Year < 1760)
            {
                return System.String.Empty;
            }
            else
            {
                if (AValue.Year > 2200)
                {
                    return "";
                }

                return AValue.ToString("dd/MM/yyyy HH:mm");
            }
        }

        public static System.String ToStringCompletoForum(System.DateTime AValue)
        {
            if (DateUtils.IsMinValue(AValue) || AValue.Year < 1760)
            {
                return System.String.Empty;
            }
            else
            {
                String data = "";
                if (AValue.Day < 10)
                {
                    data = "0";
                }
                data = data + AValue.Day + " de ";
                if (AValue.Month == 1)
                {
                    data = data + "Janeiro";
                }
                else if (AValue.Month == 2)
                {
                    data = data + "Fevereiro";
                }
                else if (AValue.Month == 3)
                {
                    data = data + "Março";
                }
                else if (AValue.Month == 4)
                {
                    data = data + "Abril";
                }
                else if (AValue.Month == 5)
                {
                    data = data + "Maio";
                }
                else if (AValue.Month == 6)
                {
                    data = data + "Junho";
                }
                else if (AValue.Month == 7)
                {
                    data = data + "Julho";
                }
                else if (AValue.Month == 8)
                {
                    data = data + "Agosto";
                }
                else if (AValue.Month == 9)
                {
                    data = data + "Setembro";
                }
                else if (AValue.Month == 10)
                {
                    data = data + "Outubro";
                }
                else if (AValue.Month == 11)
                {
                    data = data + "Novembro";
                }
                else if (AValue.Month == 12)
                {
                    data = data + "Dezembro";
                }
                data = data + " de " + AValue.Year;
                return data;
            }
        }

        public static System.String ToStringMes(System.DateTime AValue)
        {
            if (DateUtils.IsMinValue(AValue) || AValue.Year < 1760)
            {
                return System.String.Empty;
            }
            else
            {
                String data = "";
                if (AValue.Month == 1)
                {
                    data = "janeiro";
                }
                else if (AValue.Month == 2)
                {
                    data = "fevereiro";
                }
                else if (AValue.Month == 3)
                {
                    data = "março";
                }
                else if (AValue.Month == 4)
                {
                    data = "abril";
                }
                else if (AValue.Month == 5)
                {
                    data = "maio";
                }
                else if (AValue.Month == 6)
                {
                    data = "junho";
                }
                else if (AValue.Month == 7)
                {
                    data = "julho";
                }
                else if (AValue.Month == 8)
                {
                    data = "agosto";
                }
                else if (AValue.Month == 9)
                {
                    data = "setembro";
                }
                else if (AValue.Month == 10)
                {
                    data = "outubro";
                }
                else if (AValue.Month == 11)
                {
                    data = "novembro";
                }
                else if (AValue.Month == 12)
                {
                    data = "dezembro";
                }
                return data;
            }
        }

        public static System.String ToStringMesInicialMaiuscula(System.DateTime AValue)
        {
            if (DateUtils.IsMinValue(AValue) || AValue.Year < 1760)
            {
                return System.String.Empty;
            }
            else
            {
                String data = "";
                if (AValue.Month == 1)
                {
                    data = "Janeiro";
                }
                else if (AValue.Month == 2)
                {
                    data = "Fevereiro";
                }
                else if (AValue.Month == 3)
                {
                    data = "Março";
                }
                else if (AValue.Month == 4)
                {
                    data = "Abril";
                }
                else if (AValue.Month == 5)
                {
                    data = "Maio";
                }
                else if (AValue.Month == 6)
                {
                    data = "Junho";
                }
                else if (AValue.Month == 7)
                {
                    data = "Julho";
                }
                else if (AValue.Month == 8)
                {
                    data = "Agosto";
                }
                else if (AValue.Month == 9)
                {
                    data = "Setembro";
                }
                else if (AValue.Month == 10)
                {
                    data = "Outubro";
                }
                else if (AValue.Month == 11)
                {
                    data = "Novembro";
                }
                else if (AValue.Month == 12)
                {
                    data = "Dezembro";
                }
                return data;
            }
        }

        public static System.String ToStringMesAno(System.DateTime AValue)
        {
            if (DateUtils.IsMinValue(AValue) || AValue.Year < 1760)
            {
                return System.String.Empty;
            }
            else
            {
                String data = "";
                if (AValue.Month == 1)
                {
                    data = "Janeiro ";
                }
                else if (AValue.Month == 2)
                {
                    data = "Fevereiro ";
                }
                else if (AValue.Month == 3)
                {
                    data = "Março ";
                }
                else if (AValue.Month == 4)
                {
                    data = "Abril ";
                }
                else if (AValue.Month == 5)
                {
                    data = "Maio ";
                }
                else if (AValue.Month == 6)
                {
                    data = "Junho ";
                }
                else if (AValue.Month == 7)
                {
                    data = "Julho ";
                }
                else if (AValue.Month == 8)
                {
                    data = "Agosto ";
                }
                else if (AValue.Month == 9)
                {
                    data = "Setembro ";
                }
                else if (AValue.Month == 10)
                {
                    data = "Outubro ";
                }
                else if (AValue.Month == 11)
                {
                    data = "Novembro ";
                }
                else if (AValue.Month == 12)
                {
                    data = "Dezembro ";
                }
                data = data + AValue.Year;
                return data;
            }
        }

        public static System.String ToStringCompletoComSegundo(System.DateTime AValue)
        {
            if (DateUtils.IsMinValue(AValue) || AValue.Year < 1760)
            {
                return System.String.Empty;
            }
            else
            {
                return AValue.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }

        public static System.String ToStringHora(System.DateTime AValue)
        {
            if (DateUtils.IsMinValue(AValue) || AValue.Year < 1760)
            {
                return System.String.Empty;
            }
            else
            {
                return AValue.ToString("HH:mm");
            }
        }

        public static System.String ToStringCompletoDetalhado(System.DateTime AValue)
        {
            if (DateUtils.IsMinValue(AValue) || AValue.Year < 1760)
            {
                return System.String.Empty;
            }
            else
            {
                return AValue.ToString("dd/MM/yyyy") + " as " + AValue.ToString("HH:mm") + " horas";
            }
        }

        public static System.Object ToObject(System.DateTime AValue)
        {
            if (AValue != null && AValue.Year > 1755)
            {
                return AValue;
            }
            else
            {
                return new DateTime(1755, 11, 26);
            }
        }

        public static System.Boolean IsValidTime(System.String AValue)
        {
            System.DateTime dateValid;
            return (System.String.IsNullOrEmpty(AValue) || System.DateTime.TryParseExact(AValue, "HH:mm:ss", new CultureInfo("pt-BR"), DateTimeStyles.None, out dateValid));
        }

        public static Int32 GetMonthsBetween(DateTime from, DateTime to)
        {
            if (from > to) return GetMonthsBetween(to, from);

            var monthDiff = Math.Abs((to.Year * 12 + (to.Month - 1)) - (from.Year * 12 + (from.Month - 1)));

            /*if (from.AddMonths(monthDiff) > to || to.Day < from.Day)
            {
                return monthDiff - 1;
            }
            else
            {*/
            return monthDiff;
            //}
        }

    }
}
