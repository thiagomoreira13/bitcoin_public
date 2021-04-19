using System;

namespace Vinit.BitCoiner.Ent
{
    [Serializable()]
    public class EntUsuario
    {
        public static Int32 USUARIO_PADRAO = 1;

        public Int32 IdUsuario { get; set; }
        public String Usuario { get; set; }
        public String Email { get; set; }
        public String Senha { get; set; }
        public String ChaveBinance1 { get; set; }
        public String ChaveBinance2 { get; set; }
        public Boolean Ativo { get; set; }

    }
}

