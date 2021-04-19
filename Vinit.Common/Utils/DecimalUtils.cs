using System;
using System.Globalization;
using System.Linq;

namespace Vinit.Common
{
    public class DecimalUtils
    {
        public const System.String FORMAT_DECIMAL = "#,##0.00";
        public const System.String FORMAT_DECIMAL_4 = "#,####0.0000";
        public const System.String FORMAT_DECIMAL_8 = "#,####0.00000000";

        private static CultureInfo GetCultureInfo()
        {
            return CultureInfo.CreateSpecificCulture("pt-BR");
        }

        public static System.String ToString(System.Decimal AValue)
        {
            return AValue.ToString(FORMAT_DECIMAL);
        }

        public static System.String ToStringEmpty(System.Decimal AValue)
        {
            if (AValue < 0)
            {
                return "";
            }
            else
            {
                return AValue.ToString(FORMAT_DECIMAL);
            }
        }

        public static System.String ToStringAbsolute(System.Decimal AValue)
        {
            if (AValue < 0)
            {
                AValue = -AValue;
            }
            return AValue.ToString(FORMAT_DECIMAL);
        }

        public static Decimal ToDecimalRound8(System.Decimal AValue)
        {
            return Decimal.Round(AValue, 8);
        }
        public static Decimal ToDecimalRound5(System.Decimal AValue)
        {
            return Decimal.Round(AValue, 5);
        }
        public static Decimal ToDecimalRound0(System.Decimal AValue)
        {
            return Decimal.Round(AValue, 0);
        }
        public static Decimal ToDecimalRound(System.Decimal AValue, Int32 Casas)
        {
            return Decimal.Round(AValue, Casas);
        }

        public static System.String ToString_4(System.Decimal AValue)
        {
            return AValue.ToString(FORMAT_DECIMAL_4);
        }
        public static System.String ToString_8(System.Decimal AValue)
        {
            return AValue.ToString(FORMAT_DECIMAL_8);
        }

        public static System.Boolean IsValid(System.String AValue)
        {
            System.Decimal AParseValue;
            return System.Decimal.TryParse(AValue,
                                          NumberStyles.Float,
                                          CultureInfo.CreateSpecificCulture("pt-BR"),
                                          out AParseValue);
        }

        public static System.Boolean IsValid(System.String AValue, out System.Decimal AParseValue)
        {
            return System.Decimal.TryParse(AValue,
                                          NumberStyles.Float,
                                          CultureInfo.CreateSpecificCulture("pt-BR"),
                                          out AParseValue);
        }

        public static System.Object ToObject(System.Decimal AValue)
        {
            if (IntUtils.IsValid64(DecimalUtils.ToString(AValue)))
            {
                return Convert.ToString(AValue);
            }
            else
            {
                return System.String.Empty;
            }
        }

        public enum TipoValorExtenso
        {
            Monetario,
            Porcentagem,
            Decimal,
            DecimalFeminino
        }
        public static string ToExtenso(decimal valor, TipoValorExtenso tipoValorExtenso)
        {
            if (valor == 0)
            {
                return "zero";
            }
            else if (valor >= 1000000000000000)
            {
                throw new ArgumentOutOfRangeException("Valor não suportado pelo sistema. Valor: " + valor);
            }

            string strValor = String.Empty;
            strValor = valor.ToString("000000000000000.00#");
            //strValor = valor.ToString("{0:0.00#}");
            string valor_por_extenso = string.Empty;
            int qtdCasasDecimais =
                strValor.Substring(strValor.IndexOf(',') + 1, strValor.Length - (strValor.IndexOf(',') + 1)).Length;
            bool existemValoresAposDecimal = Convert.ToInt32(strValor.Substring(16, qtdCasasDecimais)) > 0;

            for (int i = 0; i <= 15; i += 3)
            {
                var parte = strValor.Substring(i, 3);
                // se parte contém vírgula, pega a substring com base na quantidade de casas decimais.
                if (parte.Contains(','))
                {
                    parte = strValor.Substring(i + 1, qtdCasasDecimais);
                }
                valor_por_extenso += escreva_parte(Convert.ToDecimal(parte), tipoValorExtenso);
                if (i == 0 & valor_por_extenso != string.Empty)
                {
                    if (Convert.ToInt32(strValor.Substring(0, 3)) == 1)
                        valor_por_extenso += " TRILHÃO" +
                                             ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0)
                                                  ? " E "
                                                  : string.Empty);
                    else if (Convert.ToInt32(strValor.Substring(0, 3)) > 1)
                        valor_por_extenso += " TRILHÕES" +
                                             ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0)
                                                  ? " E "
                                                  : string.Empty);
                }
                else if (i == 3 & valor_por_extenso != string.Empty)
                {
                    if (Convert.ToInt32(strValor.Substring(3, 3)) == 1)
                        valor_por_extenso += " BILHÃO" +
                                             ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0)
                                                  ? " E "
                                                  : string.Empty);
                    else if (Convert.ToInt32(strValor.Substring(3, 3)) > 1)
                        valor_por_extenso += " BILHÕES" +
                                             ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0)
                                                  ? " E "
                                                  : string.Empty);
                }
                else if (i == 6 & valor_por_extenso != string.Empty)
                {
                    if (Convert.ToInt32(strValor.Substring(6, 3)) == 1)
                        valor_por_extenso += " MILHÃO" +
                                             ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0)
                                                  ? " E "
                                                  : string.Empty);
                    else if (Convert.ToInt32(strValor.Substring(6, 3)) > 1)
                        valor_por_extenso += " MILHÕES" +
                                             ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0)
                                                  ? " E "
                                                  : string.Empty);
                }
                else if (i == 9 & valor_por_extenso != string.Empty)
                    if (Convert.ToInt32(strValor.Substring(9, 3)) > 0)
                        valor_por_extenso += " MIL" +
                                             ((Convert.ToDecimal(strValor.Substring(12, 3)) > 0)
                                                  ? " E "
                                                  : string.Empty);

                if (i == 12)
                {
                    if (valor_por_extenso.Length > 8)
                        if (valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "BILHÃO" |
                            valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "MILHÃO")
                            valor_por_extenso += " DE";
                        else if (valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "BILHÕES" |
                                 valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "MILHÕES" |
                                 valor_por_extenso.Substring(valor_por_extenso.Length - 8, 7) == "TRILHÕES")
                            valor_por_extenso += " DE";
                        else if (valor_por_extenso.Substring(valor_por_extenso.Length - 8, 8) == "TRILHÕES")
                            valor_por_extenso += " DE";

                    if (Convert.ToInt64(strValor.Substring(0, 15)) == 1)
                    {
                        switch (tipoValorExtenso)
                        {
                            case TipoValorExtenso.Monetario:
                                valor_por_extenso += " REAL";
                                break;
                            case TipoValorExtenso.Porcentagem:
                                if (existemValoresAposDecimal == false)
                                    valor_por_extenso += " PORCENTO";
                                break;
                            case TipoValorExtenso.Decimal:
                            case TipoValorExtenso.DecimalFeminino:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("tipoValorExtenso");
                        }
                    }

                    else if (Convert.ToInt64(strValor.Substring(0, 15)) > 1)
                    {
                        switch (tipoValorExtenso)
                        {
                            case TipoValorExtenso.Monetario:
                                valor_por_extenso += " REAIS";
                                break;
                            case TipoValorExtenso.Porcentagem:
                                if (existemValoresAposDecimal == false)
                                    valor_por_extenso += " PORCENTO";
                                break;
                            case TipoValorExtenso.Decimal:
                            case TipoValorExtenso.DecimalFeminino:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("tipoValorExtenso");
                        }
                    }

                    if (Convert.ToInt32(strValor.Substring(16, 2)) > 0 && valor_por_extenso != string.Empty)
                    {
                        switch (tipoValorExtenso)
                        {
                            case TipoValorExtenso.Monetario:
                                valor_por_extenso += " E ";
                                break;
                            case TipoValorExtenso.Porcentagem:
                                valor_por_extenso += " VÍRGULA ";
                                break;
                            case TipoValorExtenso.Decimal:
                            case TipoValorExtenso.DecimalFeminino:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("tipoValorExtenso");
                        }
                    }
                }

                if (i == 15)
                    if (Convert.ToInt32(strValor.Substring(16, qtdCasasDecimais)) == 1)
                    {
                        switch (tipoValorExtenso)
                        {
                            case TipoValorExtenso.Monetario:
                                valor_por_extenso += " CENTAVO";
                                break;
                            case TipoValorExtenso.Porcentagem:
                                valor_por_extenso += " CENTAVO";
                                break;
                            case TipoValorExtenso.Decimal:
                            case TipoValorExtenso.DecimalFeminino:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("tipoValorExtenso");
                        }
                    }

                    else if (Convert.ToInt32(strValor.Substring(16, qtdCasasDecimais)) > 1)
                    {
                        switch (tipoValorExtenso)
                        {
                            case TipoValorExtenso.Monetario:
                                valor_por_extenso += " CENTAVOS";
                                break;
                            case TipoValorExtenso.Porcentagem:
                                valor_por_extenso += " PORCENTO";
                                break;
                            case TipoValorExtenso.Decimal:
                            case TipoValorExtenso.DecimalFeminino:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("tipoValorExtenso");
                        }
                    }
            }
            return valor_por_extenso;
        }

        private static string escreva_parte(decimal valor, TipoValorExtenso tipoValor)
        {
            if (valor <= 0)
                return string.Empty;
            else
            {
                string montagem = string.Empty;
                if (valor > 0 & valor < 1)
                {
                    valor *= 100;
                }
                string strValor = valor.ToString("000");
                int a = Convert.ToInt32(strValor.Substring(0, 1));
                int b = Convert.ToInt32(strValor.Substring(1, 1));
                int c = Convert.ToInt32(strValor.Substring(2, 1));

                if (a == 1) montagem += (b + c == 0) ? "CEM" : "CENTO";
                else if (a == 2) montagem += "DUZENTOS";
                else if (a == 3) montagem += "TREZENTOS";
                else if (a == 4) montagem += "QUATROCENTOS";
                else if (a == 5) montagem += "QUINHENTOS";
                else if (a == 6) montagem += "SEISCENTOS";
                else if (a == 7) montagem += "SETECENTOS";
                else if (a == 8) montagem += "OITOCENTOS";
                else if (a == 9) montagem += "NOVECENTOS";

                if (b == 1)
                {
                    if (c == 0) montagem += ((a > 0) ? " E " : string.Empty) + "DEZ";
                    else if (c == 1) montagem += ((a > 0) ? " E " : string.Empty) + "ONZE";
                    else if (c == 2) montagem += ((a > 0) ? " E " : string.Empty) + "DOZE";
                    else if (c == 3) montagem += ((a > 0) ? " E " : string.Empty) + "TREZE";
                    else if (c == 4) montagem += ((a > 0) ? " E " : string.Empty) + "QUATORZE";
                    else if (c == 5) montagem += ((a > 0) ? " E " : string.Empty) + "QUINZE";
                    else if (c == 6) montagem += ((a > 0) ? " E " : string.Empty) + "DEZESSEIS";
                    else if (c == 7) montagem += ((a > 0) ? " E " : string.Empty) + "DEZESSETE";
                    else if (c == 8) montagem += ((a > 0) ? " E " : string.Empty) + "DEZOITO";
                    else if (c == 9) montagem += ((a > 0) ? " E " : string.Empty) + "DEZENOVE";
                }
                else if (b == 2) montagem += ((a > 0) ? " E " : string.Empty) + "VINTE";
                else if (b == 3) montagem += ((a > 0) ? " E " : string.Empty) + "TRINTA";
                else if (b == 4) montagem += ((a > 0) ? " E " : string.Empty) + "QUARENTA";
                else if (b == 5) montagem += ((a > 0) ? " E " : string.Empty) + "CINQUENTA";
                else if (b == 6) montagem += ((a > 0) ? " E " : string.Empty) + "SESSENTA";
                else if (b == 7) montagem += ((a > 0) ? " E " : string.Empty) + "SETENTA";
                else if (b == 8) montagem += ((a > 0) ? " E " : string.Empty) + "OITENTA";
                else if (b == 9) montagem += ((a > 0) ? " E " : string.Empty) + "NOVENTA";

                if (strValor.Substring(1, 1) != "1" & c != 0 & montagem != string.Empty) montagem += " E ";

                if (strValor.Substring(1, 1) != "1")
                    if (c == 1)
                    {
                        if (tipoValor == TipoValorExtenso.DecimalFeminino)
                        {
                            montagem += "UMA";
                        }
                        else
                        {
                            montagem += "UM";
                        }
                    }
                    else if (c == 2)
                    {
                        if (tipoValor == TipoValorExtenso.DecimalFeminino)
                        {
                            montagem += "DUAS";
                        }
                        else
                        {
                            montagem += "DOIS";
                        }
                    }
                    else if (c == 3) montagem += "TRÊS";
                    else if (c == 4) montagem += "QUATRO";
                    else if (c == 5) montagem += "CINCO";
                    else if (c == 6) montagem += "SEIS";
                    else if (c == 7) montagem += "SETE";
                    else if (c == 8) montagem += "OITO";
                    else if (c == 9) montagem += "NOVE";

                return montagem;
            }
        }
    }
}
