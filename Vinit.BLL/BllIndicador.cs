using System;
using System.Collections.Generic;
using System.Data.Common;
using Vinit.BitCoiner.DAL;
using Vinit.BitCoiner.Ent;

namespace Vinit.BitCoiner.BLL
{
    public class BllIndicador : BllBase
    {
        private DalIndicador dalIndicador = new DalIndicador();

        public EntIndicador Inserir(EntIndicador objIndicador, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objIndicador = dalIndicador.Inserir(objIndicador, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_INSERIR, "Indicador", objIndicador.IdIndicador, objIndicador, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Indicador", objIndicador.IdIndicador, objIndicador, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return objIndicador;
        }

        public void Alterar(EntIndicador objIndicador, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    dalIndicador.Alterar(objIndicador, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_EDITAR, "Indicador", objIndicador.IdIndicador, objIndicador, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Indicador", objIndicador.IdIndicador, objIndicador, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public EntIndicador Remover(EntIndicador objIndicador, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objIndicador.Ativo = !objIndicador.Ativo;
                    dalIndicador.Remover(objIndicador, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_REMOVER, "Indicador", objIndicador.IdIndicador, objIndicador, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Indicador", objIndicador.IdIndicador, objIndicador, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return objIndicador;
        }


        public List<EntIndicador> ObterTodos(String Indicador, Int32 Moeda, DateTime DataInicio, DateTime DataFim, Int32 Status)
        {
            List<EntIndicador> lstIndicador = new List<EntIndicador>();

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    lstIndicador = dalIndicador.ObterTodos(Indicador.Trim(), Moeda, DataInicio, DataFim, Status, transaction, db);
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
            return lstIndicador;
        }

        public EntIndicador ObterPorId(Int32 IdIndicador)
        {
            EntIndicador objIndicador = new EntIndicador();
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objIndicador = dalIndicador.ObterPorId(IdIndicador, transaction, db);
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
            return objIndicador;
        }


    }
}

