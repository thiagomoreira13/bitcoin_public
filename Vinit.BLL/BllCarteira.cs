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
    public class BllCarteira : BllBase
    {
private DalCarteira dalCarteira = new DalCarteira();

        public EntCarteira Inserir(EntCarteira objCarteira, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objCarteira = dalCarteira.Inserir(objCarteira, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_INSERIR, "Carteira", objCarteira.IdCarteira, objCarteira, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Carteira", objCarteira.IdCarteira, objCarteira, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return objCarteira;
        }

        public void Alterar(EntCarteira objCarteira, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    dalCarteira.Alterar(objCarteira, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_EDITAR, "Carteira", objCarteira.IdCarteira, objCarteira, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Carteira", objCarteira.IdCarteira, objCarteira, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public EntCarteira Remover(EntCarteira objCarteira, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objCarteira.Ativo = !objCarteira.Ativo;
                    dalCarteira.Remover(objCarteira, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_REMOVER, "Carteira", objCarteira.IdCarteira, objCarteira, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Carteira", objCarteira.IdCarteira, objCarteira, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return objCarteira;
        }


        public List<EntCarteira> ObterTodos(String Carteira, Int32 Usuario, Int32 Moeda, Int32 Status)
        {
            List<EntCarteira> lstCarteira = new List<EntCarteira>();

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    lstCarteira = dalCarteira.ObterTodos(Carteira.Trim(), Usuario, Moeda, Status, transaction, db);
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
            return lstCarteira;
        }

        public EntCarteira ObterPorId(Int32 IdCarteira)
        {
            EntCarteira objCarteira = new EntCarteira();
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objCarteira = dalCarteira.ObterPorId(IdCarteira, transaction, db);
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
            return objCarteira;
        }


    }
}

