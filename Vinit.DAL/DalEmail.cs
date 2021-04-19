using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Vinit.BitCoiner.Ent;
using Vinit.Common;

namespace Vinit.BitCoiner.DAL
{
    public class DalEmail
    {

        public EntEmail Inserir(EntEmail objEmail, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_EmailInserir");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddOutParameter(dbCommand, "@CDA_EMAIL", DbType.Int32, objEmail.IdEmail);
            db.AddInParameter(dbCommand, "@TX_EMAIL", DbType.String, objEmail.Email);
            db.AddInParameter(dbCommand, "@TX_TITULO", DbType.String, objEmail.Titulo);
            db.AddInParameter(dbCommand, "@TX_MENSAGEM", DbType.String, objEmail.Mensagem);
            db.AddInParameter(dbCommand, "@DT_DATA_ENVIO", DbType.DateTime, objEmail.DataEnvio);
            db.AddInParameter(dbCommand, "@FL_IS_ENVIO_SUCESSO", DbType.Boolean, objEmail.IsEnvioSucesso);
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objEmail.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);

            objEmail.IdEmail = ObjectUtils.ToInt(db.GetParameterValue(dbCommand, "@CDA_EMAIL"));

            return objEmail;
        }

        public void Alterar(EntEmail objEmail, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_EmailAlterar");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddInParameter(dbCommand, "@CDA_EMAIL", DbType.Int32, objEmail.IdEmail);
            db.AddInParameter(dbCommand, "@TX_EMAIL", DbType.String, objEmail.Email);
            db.AddInParameter(dbCommand, "@TX_TITULO", DbType.String, objEmail.Titulo);
            db.AddInParameter(dbCommand, "@TX_MENSAGEM", DbType.String, objEmail.Mensagem);
            db.AddInParameter(dbCommand, "@DT_DATA_ENVIO", DbType.DateTime, objEmail.DataEnvio);
            db.AddInParameter(dbCommand, "@FL_IS_ENVIO_SUCESSO", DbType.Boolean, objEmail.IsEnvioSucesso);
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objEmail.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);
        }

        public void Remover(EntEmail objEmail, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_EmailRemover");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddInParameter(dbCommand, "@CDA_EMAIL", DbType.Int32, objEmail.IdEmail);
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objEmail.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);
        }

        public List<EntEmail> ObterTodos(String Email, String Titulo, DateTime DataInicio, DateTime DataFim, Int32 IsEnvioSucesso, Int32 Status, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_EmailSelecionarTodos");
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("Email"), DbType.String, Email);
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("Titulo"), DbType.String, Titulo);
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("DataInicio"), DbType.DateTime, DataInicio);
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("DataFim"), DbType.DateTime, DataFim);
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("IsEnvioSucesso"), DbType.Int32, IntUtils.ToBooleanNullProc(IsEnvioSucesso));
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("Status"), DbType.Boolean, IntUtils.ToBooleanNullProc(Status));
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            using (DbDataReader dtrDados = (System.Data.Common.DbDataReader)db.ExecuteReader(dbCommand, transaction))
            {
                if (dtrDados != null && dtrDados.HasRows)
                {
                    return this.Popular(dtrDados);
                }
                else
                {
                    return new List<EntEmail>();
                }
            }
        }

        private List<EntEmail> Popular(DbDataReader dtrDados)
        {
            List<EntEmail> listEntReturn = new List<EntEmail>();
            EntEmail entReturn;

            try
            {
                while (dtrDados.Read())
                {
                    entReturn = new EntEmail();

                    entReturn.IdEmail = ObjectUtils.ToInt(dtrDados["CDA_EMAIL"]);
                    entReturn.Email = ObjectUtils.ToString(dtrDados["TX_EMAIL"]);
                    entReturn.Titulo = ObjectUtils.ToString(dtrDados["TX_TITULO"]);
                    entReturn.Mensagem = ObjectUtils.ToString(dtrDados["TX_MENSAGEM"]);
                    entReturn.DataEnvio = ObjectUtils.ToDate(dtrDados["DT_DATA_ENVIO"]);
                    entReturn.IsEnvioSucesso = ObjectUtils.ToBoolean(dtrDados["FL_IS_ENVIO_SUCESSO"]);
                    entReturn.Ativo = ObjectUtils.ToBoolean(dtrDados["FL_ATIVO"]);
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

        public EntEmail ObterPorId(Int32 IdEmail, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_EmailSelecionarPorIdEmail");
            db.AddInParameter(dbCommand, "@IdEmail", DbType.Int32, IdEmail);
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            using (DbDataReader dtrDados = (System.Data.Common.DbDataReader)db.ExecuteReader(dbCommand, transaction))
            {
                if (dtrDados != null && dtrDados.HasRows)
                {
                    return this.Popular(dtrDados)[0];
                }
                else
                {
                    return new EntEmail();
                }
            }
        }


    }
}

