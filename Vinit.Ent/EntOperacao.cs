using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vinit.BitCoiner.Ent
{
    [Serializable()]
    public class EntOperacao
    {
        public Int32 IdOperacao { get; set; }
        public String Operacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataInicioExecucao { get; set; }
        public DateTime DataFimExecucao { get; set; }
        public Decimal ValorReais { get; set; }
        public Decimal ValorDolar { get; set; }
        public Decimal ValorBitcoin { get; set; }
        private EntCarteira _Carteira;
        public EntCarteira Carteira
        {
            get
            {
                if (_Carteira == null)
                {
                    _Carteira = new EntCarteira();
                }
                return _Carteira;
            }

            set { _Carteira = value; }
        }
        public Boolean IsVenda { get; set; }
        public Decimal Taxa { get; set; }
        public Boolean Ativo { get; set; }

    }
}

