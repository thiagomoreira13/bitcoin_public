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
    public class BllOperacao : BllBase
    {
private DalOperacao dalOperacao = new DalOperacao();

        public EntOperacao Inserir(EntOperacao objOperacao, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objOperacao = dalOperacao.Inserir(objOperacao, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_INSERIR, "Operação", objOperacao.IdOperacao, objOperacao, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Operação", objOperacao.IdOperacao, objOperacao, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return objOperacao;
        }

        public void Alterar(EntOperacao objOperacao, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    dalOperacao.Alterar(objOperacao, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_EDITAR, "Operação", objOperacao.IdOperacao, objOperacao, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Operação", objOperacao.IdOperacao, objOperacao, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public EntOperacao Remover(EntOperacao objOperacao, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objOperacao.Ativo = !objOperacao.Ativo;
                    dalOperacao.Remover(objOperacao, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_REMOVER, "Operação", objOperacao.IdOperacao, objOperacao, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Operação", objOperacao.IdOperacao, objOperacao, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return objOperacao;
        }


        public List<EntOperacao> ObterTodos(String Operacao, Int32 Carteira, Int32 IsVenda, Int32 Status)
        {
            List<EntOperacao> lstOperacao = new List<EntOperacao>();

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    lstOperacao = dalOperacao.ObterTodos(Operacao.Trim(), Carteira, IsVenda, Status, transaction, db);
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
            return lstOperacao;
        }

        public EntOperacao ObterPorId(Int32 IdOperacao)
        {
            EntOperacao objOperacao = new EntOperacao();
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objOperacao = dalOperacao.ObterPorId(IdOperacao, transaction, db);
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
            return objOperacao;
        }


    }
}

