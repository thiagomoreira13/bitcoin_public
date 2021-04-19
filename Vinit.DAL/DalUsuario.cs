using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Vinit.BitCoiner.Ent;
using Vinit.Common;

namespace Vinit.BitCoiner.DAL
{
    public class DalUsuario
    {

        public EntUsuario Inserir(EntUsuario objUsuario, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_UsuarioInserir");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddOutParameter(dbCommand, "@CDA_USUARIO", DbType.Int32, objUsuario.IdUsuario);
            db.AddInParameter(dbCommand, "@TX_USUARIO", DbType.String, objUsuario.Usuario);
            db.AddInParameter(dbCommand, "@TX_EMAIL", DbType.String, objUsuario.Email);
            db.AddInParameter(dbCommand, "@TX_SENHA", DbType.String, objUsuario.Senha);
            db.AddInParameter(dbCommand, "@TX_CHAVE_BINANCE_1", DbType.String, objUsuario.ChaveBinance1);
            db.AddInParameter(dbCommand, "@TX_CHAVE_BINANCE_2", DbType.String, objUsuario.ChaveBinance2);
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objUsuario.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);

            objUsuario.IdUsuario = ObjectUtils.ToInt(db.GetParameterValue(dbCommand, "@CDA_USUARIO"));

            return objUsuario;
        }

        public void AlterarSenha(EntUsuario objUsuario, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_UsuarioAlterarSenha");

            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddInParameter(dbCommand, "@CDA_USUARIO", DbType.Int32, objUsuario.IdUsuario);
            db.AddInParameter(dbCommand, "@TX_SENHA", DbType.String, objUsuario.Senha);

            db.ExecuteNonQuery(dbCommand, transaction);
        }

        public void Alterar(EntUsuario objUsuario, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_UsuarioAlterar");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddInParameter(dbCommand, "@CDA_USUARIO", DbType.Int32, objUsuario.IdUsuario);
            db.AddInParameter(dbCommand, "@TX_USUARIO", DbType.String, objUsuario.Usuario);
            db.AddInParameter(dbCommand, "@TX_EMAIL", DbType.String, objUsuario.Email);
            db.AddInParameter(dbCommand, "@TX_SENHA", DbType.String, objUsuario.Senha);
            db.AddInParameter(dbCommand, "@TX_CHAVE_BINANCE_1", DbType.String, objUsuario.ChaveBinance1);
            db.AddInParameter(dbCommand, "@TX_CHAVE_BINANCE_2", DbType.String, objUsuario.ChaveBinance2);
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objUsuario.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);
        }

        public void Remover(EntUsuario objUsuario, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_UsuarioRemover");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddInParameter(dbCommand, "@CDA_USUARIO", DbType.Int32, objUsuario.IdUsuario);
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objUsuario.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);
        }

        public List<EntUsuario> ObterTodos(String Usuario, String Email, Int32 Status, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_UsuarioSelecionarTodos");
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("Usuario"), DbType.String, Usuario);
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("Email"), DbType.String, Email);
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
                    return new List<EntUsuario>();
                }
            }
        }

        private List<EntUsuario> Popular(DbDataReader dtrDados)
        {
            List<EntUsuario> listEntReturn = new List<EntUsuario>();
            EntUsuario entReturn;

            try
            {
                while (dtrDados.Read())
                {
                    entReturn = new EntUsuario();

                    entReturn.IdUsuario = ObjectUtils.ToInt(dtrDados["CDA_USUARIO"]);
                    entReturn.Usuario = ObjectUtils.ToString(dtrDados["TX_USUARIO"]);
                    entReturn.Email = ObjectUtils.ToString(dtrDados["TX_EMAIL"]);
                    entReturn.Senha = ObjectUtils.ToString(dtrDados["TX_SENHA"]);
                    entReturn.ChaveBinance1 = ObjectUtils.ToString(dtrDados["TX_CHAVE_BINANCE_1"]);
                    entReturn.ChaveBinance2 = ObjectUtils.ToString(dtrDados["TX_CHAVE_BINANCE_2"]);
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

        public EntUsuario ObterPorId(Int32 IdUsuario, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_UsuarioSelecionarPorIdUsuario");
            db.AddInParameter(dbCommand, "@IdUsuario", DbType.Int32, IdUsuario);
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            using (DbDataReader dtrDados = (System.Data.Common.DbDataReader)db.ExecuteReader(dbCommand, transaction))
            {
                if (dtrDados != null && dtrDados.HasRows)
                {
                    return this.Popular(dtrDados)[0];
                }
                else
                {
                    return new EntUsuario();
                }
            }
        }

        public EntUsuario ObterPorEmail(String Email, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_UsuarioSelecionarPorEmail");
            db.AddInParameter(dbCommand, "@Email", DbType.String, Email);
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            using (DbDataReader dtrDados = (System.Data.Common.DbDataReader)db.ExecuteReader(dbCommand, transaction))
            {
                if (dtrDados != null && dtrDados.HasRows)
                {
                    return this.Popular(dtrDados)[0];
                }
                else
                {
                    return new EntUsuario();
                }
            }
        }

    }
}