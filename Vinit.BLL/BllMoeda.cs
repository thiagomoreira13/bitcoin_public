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
    public class BllMoeda : BllBase
    {
private DalMoeda dalMoeda = new DalMoeda();

        public EntMoeda Inserir(EntMoeda objMoeda, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objMoeda = dalMoeda.Inserir(objMoeda, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_INSERIR, "Moeda", objMoeda.IdMoeda, objMoeda, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Moeda", objMoeda.IdMoeda, objMoeda, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return objMoeda;
        }

        public void Alterar(EntMoeda objMoeda, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    dalMoeda.Alterar(objMoeda, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_EDITAR, "Moeda", objMoeda.IdMoeda, objMoeda, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Moeda", objMoeda.IdMoeda, objMoeda, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public EntMoeda Remover(EntMoeda objMoeda, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objMoeda.Ativo = !objMoeda.Ativo;
                    dalMoeda.Remover(objMoeda, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_REMOVER, "Moeda", objMoeda.IdMoeda, objMoeda, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Moeda", objMoeda.IdMoeda, objMoeda, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return objMoeda;
        }


        public List<EntMoeda> ObterTodos(String Moeda, String Codigo, Int32 Status)
        {
            List<EntMoeda> lstMoeda = new List<EntMoeda>();

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    lstMoeda = dalMoeda.ObterTodos(Moeda.Trim(), Codigo.Trim(), Status, transaction, db);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return lstMoeda;
        }

        public EntMoeda ObterPorId(Int32 IdMoeda)
        {
            EntMoeda objMoeda = new EntMoeda();
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objMoeda = dalMoeda.ObterPorId(IdMoeda, transaction, db);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return objMoeda;
        }


    }
}

