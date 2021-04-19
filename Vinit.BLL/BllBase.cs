using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Vinit.BitCoiner.DAL;
using Vinit.BitCoiner.Ent;

namespace Vinit.BitCoiner.BLL
{
    public class BllBase
    {
        public static Database db;
        private DalLog dalLog = new DalLog();

        public BllBase()
        {
            try
            {
                db = DatabaseFactory.CreateDatabase("Vinit");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void SaveLog(Int32 IdUsuario, Int32 TipoLog, String Log, Int32 IdEntidade, object Entidade, String Erro, Database db, DbConnection connection)
        {
        DbTransaction transaction = connection.BeginTransaction();
            try
            {
                EntLog log = new EntLog();
                log.DateEvento = DateTime.Now;
                log.Usuario.IdUsuario = IdUsuario;
                log.TipoLog = TipoLog;
                log.Log = Log + " " + EntLog.GetMyProperties(Entidade);
                if (IdEntidade > 0)
                {
                    log.Log = log.Log + " (ID " + IdEntidade + "): " + Erro;
                }
                dalLog.Inserir(log, transaction, db);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        protected void SaveLog(Int32 IdUsuario, Int32 TipoLog, String Log, Int32 IdEntidade, object Entidade, DbTransaction transaction, Database db)
        {
            EntLog log = new EntLog();
            log.DateEvento = DateTime.Now;
            log.Usuario.IdUsuario = IdUsuario;
            log.TipoLog = TipoLog;
            log.Log = Log + " " + EntLog.GetMyProperties(Entidade);
            if (IdEntidade > 0)
            {
                log.Log = log.Log + " (ID " + IdEntidade + ")";
            }
            dalLog.Inserir(log, transaction, db);
        }
		
    }
}

