using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Vinit.BitCoiner.Ent;
using Vinit.Common;

namespace Vinit.BitCoiner.DAL
{
    public class DalMoeda
    {

        public EntMoeda Inserir(EntMoeda objMoeda, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_MoedaInserir");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddOutParameter(dbCommand, "@CDA_MOEDA", DbType.Int32, objMoeda.IdMoeda);
            db.AddInParameter(dbCommand, "@TX_MOEDA", DbType.String, objMoeda.Moeda);
            db.AddInParameter(dbCommand, "@TX_CODIGO", DbType.String, objMoeda.Codigo);
            db.AddInParameter(dbCommand, "@CEA_OPERACAO_COMPRA", DbType.Int32, IntUtils.ToIntNullProc(objMoeda.OperacaoCompraAberta.IdOperacao));
            db.AddInParameter(dbCommand, "@CEA_OPERACAO_VENDA", DbType.Int32, IntUtils.ToIntNullProc(objMoeda.OperacaoVendaAberta.IdOperacao));
            db.AddInParameter(dbCommand, "@NU_CASAS_APOS_VIRGULA", DbType.Int32, objMoeda.CasasDepoisDaVirgula);
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objMoeda.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);

            objMoeda.IdMoeda = ObjectUtils.ToInt(db.GetParameterValue(dbCommand, "@CDA_MOEDA"));

            return objMoeda;
        }

        public void Alterar(EntMoeda objMoeda, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_MoedaAlterar");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddInParameter(dbCommand, "@CDA_MOEDA", DbType.Int32, objMoeda.IdMoeda);
            db.AddInParameter(dbCommand, "@TX_MOEDA", DbType.String, objMoeda.Moeda);
            db.AddInParameter(dbCommand, "@TX_CODIGO", DbType.String, objMoeda.Codigo);
            db.AddInParameter(dbCommand, "@CEA_OPERACAO_COMPRA", DbType.Int32, IntUtils.ToIntNullProc(objMoeda.OperacaoCompraAberta.IdOperacao));
            db.AddInParameter(dbCommand, "@CEA_OPERACAO_VENDA", DbType.Int32, IntUtils.ToIntNullProc(objMoeda.OperacaoVendaAberta.IdOperacao));
            db.AddInParameter(dbCommand, "@NU_CASAS_APOS_VIRGULA", DbType.Int32, objMoeda.CasasDepoisDaVirgula);
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objMoeda.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);
        }

        public void Remover(EntMoeda objMoeda, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_MoedaRemover");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddInParameter(dbCommand, "@CDA_MOEDA", DbType.Int32, objMoeda.IdMoeda);
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objMoeda.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);
        }

        public List<EntMoeda> ObterTodos(String Moeda, String Codigo, Int32 Status, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_MoedaSelecionarTodos");
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("Moeda"), DbType.String, Moeda);
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("Codigo"), DbType.String, Codigo);
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
                    return new List<EntMoeda>();
                }
            }
        }

        private List<EntMoeda> Popular(DbDataReader dtrDados)
        {
            List<EntMoeda> listEntReturn = new List<EntMoeda>();
            EntMoeda entReturn;

            try
            {
                while (dtrDados.Read())
                {
                    entReturn = new EntMoeda();

                    entReturn.IdMoeda = ObjectUtils.ToInt(dtrDados["CDA_MOEDA"]);
                    entReturn.Moeda = ObjectUtils.ToString(dtrDados["TX_MOEDA"]);
                    entReturn.Codigo = ObjectUtils.ToString(dtrDados["TX_CODIGO"]);
                    entReturn.OperacaoCompraAberta.IdOperacao = ObjectUtils.ToInt(dtrDados["CEA_OPERACAO_COMPRA"]);
                    entReturn.OperacaoVendaAberta.IdOperacao = ObjectUtils.ToInt(dtrDados["CEA_OPERACAO_VENDA"]);
                    entReturn.CasasDepoisDaVirgula = ObjectUtils.ToInt(dtrDados["NU_CASAS_APOS_VIRGULA"]);
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

        public EntMoeda ObterPorId(Int32 IdMoeda, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_MoedaSelecionarPorIdMoeda");
            db.AddInParameter(dbCommand, "@IdMoeda", DbType.Int32, IdMoeda);
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            using (DbDataReader dtrDados = (System.Data.Common.DbDataReader)db.ExecuteReader(dbCommand, transaction))
            {
                if (dtrDados != null && dtrDados.HasRows)
                {
                    return this.Popular(dtrDados)[0];
                }
                else
                {
                    return new EntMoeda();
                }
            }
        }


    }
}