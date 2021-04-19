using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Vinit.Common;
using Vinit.BitCoiner.DAL;
using Vinit.BitCoiner.Ent;

namespace Vinit.BitCoiner.BLL
{
    public class BllLog : BllBase
    {
private DalLog dalLog = new DalLog();


        public EntLog Inserir(EntLog objLog)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    objLog = dalLog.Inserir(objLog, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }

            return objLog;
        }

        public List<EntLog> ObterTodos(String Log, Int32 IdUsuario, Int32 TipoLog, DateTime DataInicio, DateTime DataFim)
        {
            List<EntLog> lstLog = new List<EntLog>();

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    lstLog = dalLog.ObterTodos(Log, IdUsuario, TipoLog, DataInicio, DataFim, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return lstLog;
        }

    }
}

