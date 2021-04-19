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
    public class BllUsuario : BllBase
    {
        private DalUsuario dalUsuario = new DalUsuario();

        public EntUsuario Inserir(EntUsuario objUsuario, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objUsuario = dalUsuario.Inserir(objUsuario, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_INSERIR, "Usuário", objUsuario.IdUsuario, objUsuario, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Usuário", objUsuario.IdUsuario, objUsuario, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return objUsuario;
        }

        public void Alterar(EntUsuario objUsuario, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    dalUsuario.Alterar(objUsuario, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_EDITAR, "Usuário", objUsuario.IdUsuario, objUsuario, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Usuário", objUsuario.IdUsuario, objUsuario, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void AlterarSenha(EntUsuario objUsuario, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    dalUsuario.AlterarSenha(objUsuario, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_EDITAR, "Usuário", objUsuario.IdUsuario, objUsuario, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Usuário", objUsuario.IdUsuario, objUsuario, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public EntUsuario Remover(EntUsuario objUsuario, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objUsuario.Ativo = !objUsuario.Ativo;
                    dalUsuario.Remover(objUsuario, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_REMOVER, "Usuário", objUsuario.IdUsuario, objUsuario, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "Usuário", objUsuario.IdUsuario, objUsuario, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return objUsuario;
        }


        public List<EntUsuario> ObterTodos(String Usuario, String Email, Int32 Status)
        {
            List<EntUsuario> lstUsuario = new List<EntUsuario>();

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    lstUsuario = dalUsuario.ObterTodos(Usuario.Trim(), Email.Trim(), Status, transaction, db);
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
            return lstUsuario;
        }

        public EntUsuario ObterPorId(Int32 IdUsuario)
        {
            EntUsuario objUsuario = new EntUsuario();
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objUsuario = dalUsuario.ObterPorId(IdUsuario, transaction, db);
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
            return objUsuario;
        }

        public EntUsuario ObterPorEmail(String Email)
        {
            EntUsuario objUsuario = new EntUsuario();
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objUsuario = dalUsuario.ObterPorEmail(Email, transaction, db);
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
            return objUsuario;
        }

    }
}