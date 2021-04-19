using System;
using System.Collections.Generic;
using System.Data.Common;
using Vinit.BitCoiner.DAL;
using Vinit.BitCoiner.Ent;

namespace Vinit.BitCoiner.BLL
{
    public class BllEmail : BllBase
    {
        private DalEmail dalEmail = new DalEmail();

        public EntEmail Inserir(EntEmail objEmail, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objEmail = dalEmail.Inserir(objEmail, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_INSERIR, "E-mail", objEmail.IdEmail, objEmail, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "E-mail", objEmail.IdEmail, objEmail, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return objEmail;
        }

        public void Alterar(EntEmail objEmail, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    dalEmail.Alterar(objEmail, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_EDITAR, "E-mail", objEmail.IdEmail, objEmail, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "E-mail", objEmail.IdEmail, objEmail, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public EntEmail Remover(EntEmail objEmail, Int32 IdUsuario)
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objEmail.Ativo = !objEmail.Ativo;
                    dalEmail.Remover(objEmail, transaction, db);
                    SaveLog(IdUsuario, EntLog.LOG_REMOVER, "E-mail", objEmail.IdEmail, objEmail, transaction, db);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    SaveLog(IdUsuario, EntLog.LOG_ERRO, "E-mail", objEmail.IdEmail, objEmail, ex.Message, db, connection);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return objEmail;
        }


        public List<EntEmail> ObterTodos(String Email, String Titulo, DateTime DataInicio, DateTime DataFim, Int32 IsEnvioSucesso, Int32 Status)
        {
            List<EntEmail> lstEmail = new List<EntEmail>();

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    lstEmail = dalEmail.ObterTodos(Email.Trim(), Titulo.Trim(), DataInicio, DataFim, IsEnvioSucesso, Status, transaction, db);
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
            return lstEmail;
        }

        public EntEmail ObterPorId(Int32 IdEmail)
        {
            EntEmail objEmail = new EntEmail();
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    objEmail = dalEmail.ObterPorId(IdEmail, transaction, db);
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
            return objEmail;
        }


    }
}

