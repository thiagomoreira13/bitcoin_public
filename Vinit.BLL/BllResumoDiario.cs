using System;
using System.Collections.Generic;
using System.Data.Common;
using Vinit.BitCoiner.DAL;
using Vinit.BitCoiner.Ent;

namespace Vinit.BitCoiner.BLL
{
    public class BllResumoDiario : BllBase
    {
        private DalResumoDiario dalResumoDiario = new DalResumoDiario();

        public EntResumoDiario Inserir(EntResumoDiario objResumoDiario, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objResumoDiario = dalResumoDiario.Inserir(objResumoDiario, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_INSERIR, "Resumo Diário", objResumoDiario.IdResumoDiario, objResumoDiario, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Resumo Diário", objResumoDiario.IdResumoDiario, objResumoDiario, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return objResumoDiario;
        }

        public void Alterar(EntResumoDiario objResumoDiario, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    dalResumoDiario.Alterar(objResumoDiario, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_EDITAR, "Resumo Diário", objResumoDiario.IdResumoDiario, objResumoDiario, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Resumo Diário", objResumoDiario.IdResumoDiario, objResumoDiario, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public EntResumoDiario Remover(EntResumoDiario objResumoDiario, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objResumoDiario.Ativo = !objResumoDiario.Ativo;
                    dalResumoDiario.Remover(objResumoDiario, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_REMOVER, "Resumo Diário", objResumoDiario.IdResumoDiario, objResumoDiario, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Resumo Diário", objResumoDiario.IdResumoDiario, objResumoDiario, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return objResumoDiario;
        }


        public List<EntResumoDiario> ObterTodos(Int32 Moeda, DateTime DataInicio, DateTime DataFim, Int32 Status)
        {
            List<EntResumoDiario> lstResumoDiario = new List<EntResumoDiario>();

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    lstResumoDiario = dalResumoDiario.ObterTodos(Moeda, DataInicio, DataFim, Status, transaction, db);
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
            return lstResumoDiario;
        }

        public EntResumoDiario ObterPorId(Int32 IdResumoDiario)
        {
            EntResumoDiario objResumoDiario = new EntResumoDiario();
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objResumoDiario = dalResumoDiario.ObterPorId(IdResumoDiario, transaction, db);
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
            return objResumoDiario;
        }


    }
}

