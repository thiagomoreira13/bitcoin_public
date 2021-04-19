using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vinit.BitCoiner.Ent
{
    [Serializable()]
    public class EntEmail
    {
        public Int32 IdEmail { get; set; }
        public String Email { get; set; }
        public String Titulo { get; set; }
        public String Mensagem { get; set; }
        public DateTime DataEnvio { get; set; }
        public Boolean IsEnvioSucesso { get; set; }
        public Boolean Ativo { get; set; }

    }
}

