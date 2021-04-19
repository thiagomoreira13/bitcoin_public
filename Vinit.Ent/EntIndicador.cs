using System;

namespace Vinit.BitCoiner.Ent
{
    [Serializable()]
    public class EntIndicador
    {
        public Int32 IdIndicador { get; set; }
        public String Indicador { get; set; }
        public DateTime Data { get; set; }
        public Decimal Valor { get; set; }
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

