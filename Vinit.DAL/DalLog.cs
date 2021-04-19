using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Vinit.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Vinit.BitCoiner.Ent;

namespace Vinit.BitCoiner.DAL
{
    public class DalLog
    {

        public void Backup(String caminhoBackup, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_Backup");
            String database = db.ConnectionString.Substring(db.ConnectionString.IndexOf("Initial Catalog=") + 16);
            database = database.Substring(0, database.IndexOf(";"));
            db.AddInParameter(dbCommand, "@Database", DbType.String, database);
            db.AddInParameter(dbCommand, "@CaminhoBackup", DbType.String, caminhoBackup + database + ".bak");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;
            db.ExecuteNonQuery(dbCommand);
        }

        public EntLog Inserir(EntLog objLog, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_LogInserir");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddOutParameter(dbCommand, "@CDA_LOG", DbType.Int32, objLog.IdLog);
            db.AddInParameter(dbCommand, "@TX_LOG", DbType.String, objLog.Log);
            db.AddInParameter(dbCommand, "@NU_TIPO_LOG", DbType.Int32, IntUtils.ToIntNullProc(objLog.TipoLog));
            db.AddInParameter(dbCommand, "@DT_EVENTO", DbType.DateTime, DateTime.Now);
            db.AddInParameter(dbCommand, "@CEA_USUARIO", DbType.Int32, IntUtils.ToIntNullProc(objLog.Usuario.IdUsuario));

            db.ExecuteNonQuery(dbCommand, transaction);

            objLog.IdLog = ObjectUtils.ToInt(db.GetParameterValue(dbCommand, "@CDA_LOG"));

            return objLog;
        }

        public List<EntLog> ObterTodos(String Log, Int32 IdUsuario, Int32 TipoLog, DateTime DateInicio, DateTime DateFim, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_LogSelecionarTodos");
            db.AddInParameter(dbCommand, "@Log", DbType.String, Log);
            db.AddInParameter(dbCommand, "@IdUsuario", DbType.Int32, IntUtils.ToIntNullProc(IdUsuario));
            db.AddInParameter(dbCommand, "@TipoLog", DbType.Int32, IntUtils.ToIntNullProc(TipoLog));
            db.AddInParameter(dbCommand, "@DateInicio", DbType.DateTime, DateInicio);
            db.AddInParameter(dbCommand, "@DateFim", DbType.DateTime, DateFim);

            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            using (DbDataReader dtrDados = (System.Data.Common.DbDataReader)db.ExecuteReader(dbCommand, transaction))
            {
                if (dtrDados != null && dtrDados.HasRows)
                {
                    return this.Popular(dtrDados);
                }
                else
                {
                    return new List<EntLog>();
                }
            }
        }

        private List<EntLog> Popular(DbDataReader dtrDados)
        {
            List<EntLog> listEntReturn = new List<EntLog>();
            EntLog entReturn;

            try
            {
                while (dtrDados.Read())
                {
                    entReturn = new EntLog();

                    entReturn.IdLog = ObjectUtils.ToInt(dtrDados["CDA_LOG"]);
                    entReturn.Log = ObjectUtils.ToString(dtrDados["TX_LOG"]);
                    entReturn.TipoLog = ObjectUtils.ToInt(dtrDados["NU_TIPO_LOG"]);
                    entReturn.DateEvento = ObjectUtils.ToDate(dtrDados["DT_EVENTO"]);
                    entReturn.Usuario.IdUsuario = ObjectUtils.ToInt(dtrDados["CEA_USUARIO"]);
                    entReturn.Usuario.Usuario = ObjectUtils.ToString(dtrDados["TX_USUARIO"]);
                    listEntReturn.Add(entReturn);
                }

                dtrDados.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listEntReturn;
        }

    }
}

