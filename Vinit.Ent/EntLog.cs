using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Vinit.BitCoiner.Ent
{
    [Serializable()]
    public class EntLog
    {
        public const Int32 LOG_VISUALIZAR = 1;
        public const Int32 LOG_EDITAR = 2;
        public const Int32 LOG_REMOVER = 3;
        public const Int32 LOG_INSERIR = 4;
        public const Int32 LOG_ERRO = 5;

        public Int32 IdLog { get; set; }
        public String Log { get; set; }
        public Int32 TipoLog { get; set; }
        public DateTime DateEvento { get; set; }
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
        public String Acao
        {
            get
            {
                String res = "";
                if (TipoLog == EntLog.LOG_VISUALIZAR)
                {
                    res = "Visualizou ";
                }
                else if (TipoLog == EntLog.LOG_INSERIR)
                {
                    res = "Inseriu ";
                }
                else if (TipoLog == EntLog.LOG_EDITAR)
                {
                    res = "Editou ";
                }
                else if (TipoLog == EntLog.LOG_REMOVER)
                {
                    res = "Removeu ";
                }
                res = res + Log;
                return res;
            }
        }

        public static String GetMyProperties(object obj)
        {
            String res = "";
            foreach (PropertyInfo pinfo in obj.GetType().GetProperties())
            {
                var getMethod = pinfo.GetGetMethod();
                if (getMethod.ReturnType.IsArray)
                {
                    var arrayObject = getMethod.Invoke(obj, null);
                    foreach (object element in (Array)arrayObject)
                    {
                        foreach (PropertyInfo arrayObjPinfo in element.GetType().GetProperties())
                        {
                            res = res + arrayObjPinfo.Name + ": " + arrayObjPinfo.GetGetMethod().Invoke(element, null).ToString() + "\n";
                        }
                    }
                }
            }
            return res;
        }
    }
}

