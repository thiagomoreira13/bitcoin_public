using System;

namespace Vinit.BitCoiner.Ent
{
    [Serializable()]
    public class EntCarteira
    {
        public Int32 IdCarteira { get; set; }
        public String Carteira { get; set; }
        public String CarteiraUsuario
        {
            get
            {
                return Usuario.Usuario + " - " + Carteira;
            }
        }
        private EntUsuario _Usuario;
        public EntUsuario Usuario
        {
            get
            {
                if (_Usuario == null)
                {
                    _Usuario = new EntUsuario();
                }
                return _Usuario;
            }

            set { _Usuario = value; }
        }
        public Decimal SaldoReais { get; set; }
        public Decimal SaldoDolar { get; set; }
        public Decimal SaldoBitcoin { get; set; }
        public Decimal SaldoReais2 { get; set; }
        public Decimal SaldoDolar2 { get; set; }
        public Decimal SaldoBitcoin2 { get; set; }
        public Decimal PercentualTaxaCompra { get; set; }
        public Decimal PercentualTaxaVenda { get; set; }
        public Decimal PercentualLucro { get; set; }
        private EntMoeda _Moeda;
        public EntMoeda Moeda
        {
            get
            {
                if (_Moeda == null)
                {
                    _Moeda = new EntMoeda();
                }
                return _Moeda;
            }

            set { _Moeda = value; }
        }
        public Boolean Ativo { get; set; }

    }
}

