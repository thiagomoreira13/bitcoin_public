using System;

namespace Vinit.BitCoiner.Ent
{
    [Serializable()]
    public class EntResumoDiario
    {
        public DateTime DataCompra { get; set; }
        public DateTime DataVenda { get; set; }
        public Decimal ValorUnitarioCompra { get; set; }
        public Decimal ValorUnitarioVenda { get; set; }
        public Decimal ValorInicial { get; set; }
        public Decimal Lucro
        {
            get
            {
                return ((ValorUnitarioVenda - ValorUnitarioCompra) / ValorUnitarioCompra) * 100;
            }
        }
        public Decimal LucroDolar
        {
            get
            {
                return (ValorUnitarioVenda - ValorUnitarioCompra) * Quantidade;
            }
        }
        public Decimal LucroAcumulado
        {
            get
            {
                if (ValorInicial > 0)
                {
                    return ((ValorUnitarioVenda - ValorInicial) / ValorInicial) * 100;
                }
                else
                {
                    return 0;
                }
            }
        }
        public Decimal LucroDolarAcumulado
        {
            get
            {
                return (ValorUnitarioVenda - ValorInicial) * Quantidade;
            }
        }
        public Decimal ValorTotalCompra
        {
            get
            {
                return ValorUnitarioCompra * Quantidade;
            }
        }
        public Decimal ValorTotalVenda
        {
            get
            {
                return ValorUnitarioVenda * Quantidade;
            }
        }

        public Int32 IdResumoDiario { get; set; }
        public String ResumoDiario { get; set; }
        public Decimal Quantidade { get; set; }
        public Decimal Volume { get; set; }
        public Decimal PrecoUltimaTransacao { get; set; }
        public Decimal MelhorCompra { get; set; }
        public Decimal MelhorVenda { get; set; }
        public Decimal ValorMaximo { get; set; }
        public Decimal ValorMinimo { get; set; }
        public DateTime Data { get; set; }
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

