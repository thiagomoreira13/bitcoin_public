using System;

namespace Vinit.BitCoiner.Ent
{
    [Serializable()]
    public class EntMoeda
    {
        public static Int32 MOEDA_BTC = 3;
        public static Int32 MOEDA_ETH = 4;

        public static String USDT = "USDT";

        public Int32 IdMoeda { get; set; }
        public String Moeda { get; set; }
        public String Codigo { get; set; }
        public Boolean Ativo { get; set; }
        public Int32 CasasDepoisDaVirgula { get; set; }

        private EntOperacao _OperacaoCompraAberta;
        public EntOperacao OperacaoCompraAberta
        {
            get
            {
                if (_OperacaoCompraAberta == null)
                {
                    _OperacaoCompraAberta = new EntOperacao();
                }
                return _OperacaoCompraAberta;
            }

            set { _OperacaoCompraAberta = value; }
        }
        private EntOperacao _OperacaoVendaAberta;
        public EntOperacao OperacaoVendaAberta
        {
            get
            {
                if (_OperacaoVendaAberta == null)
                {
                    _OperacaoVendaAberta = new EntOperacao();
                }
                return _OperacaoVendaAberta;
            }

            set { _OperacaoVendaAberta = value; }
        }

    }
}

