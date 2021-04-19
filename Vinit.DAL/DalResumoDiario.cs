using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Vinit.BitCoiner.Ent;
using Vinit.Common;

namespace Vinit.BitCoiner.DAL
{
    public class DalResumoDiario
    {

        public EntResumoDiario Inserir(EntResumoDiario objResumoDiario, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_ResumoDiarioInserir");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddOutParameter(dbCommand, "@CDA_RESUMO_DIARIO", DbType.Int32, objResumoDiario.IdResumoDiario);
            db.AddInParameter(dbCommand, "@TX_RESUMO_DIARIO", DbType.String, objResumoDiario.ResumoDiario);
            db.AddInParameter(dbCommand, "@QUANTIDADE", DbType.Decimal, objResumoDiario.Quantidade);
            db.AddInParameter(dbCommand, "@MELHOR_COMPRA", DbType.Decimal, objResumoDiario.MelhorCompra);
            db.AddInParameter(dbCommand, "@MELHOR_VENDA", DbType.Decimal, objResumoDiario.MelhorVenda);
            db.AddInParameter(dbCommand, "@VOLUME", DbType.Decimal, objResumoDiario.Volume);
            db.AddInParameter(dbCommand, "@PRECO_ULTIMA_TRANSACAO", DbType.Decimal, objResumoDiario.PrecoUltimaTransacao);
            db.AddInParameter(dbCommand, "@VALOR_MAXIMO", DbType.Decimal, objResumoDiario.ValorMaximo);
            db.AddInParameter(dbCommand, "@VALOR_MINIMO", DbType.Decimal, objResumoDiario.ValorMinimo);
            db.AddInParameter(dbCommand, "@DT_DATA", DbType.DateTime, objResumoDiario.Data);
            db.AddInParameter(dbCommand, "@CEA_MOEDA", DbType.Int32, IntUtils.ToIntNullProc(objResumoDiario.Moeda.IdMoeda));
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objResumoDiario.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);

            objResumoDiario.IdResumoDiario = ObjectUtils.ToInt(db.GetParameterValue(dbCommand, "@CDA_RESUMO_DIARIO"));

            return objResumoDiario;
        }

        public void Alterar(EntResumoDiario objResumoDiario, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_ResumoDiarioAlterar");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddInParameter(dbCommand, "@CDA_RESUMO_DIARIO", DbType.Int32, objResumoDiario.IdResumoDiario);
            db.AddInParameter(dbCommand, "@TX_RESUMO_DIARIO", DbType.String, objResumoDiario.ResumoDiario);
            db.AddInParameter(dbCommand, "@QUANTIDADE", DbType.Decimal, objResumoDiario.Quantidade);
            db.AddInParameter(dbCommand, "@MELHOR_COMPRA", DbType.Decimal, objResumoDiario.MelhorCompra);
            db.AddInParameter(dbCommand, "@MELHOR_VENDA", DbType.Decimal, objResumoDiario.MelhorVenda);
            db.AddInParameter(dbCommand, "@VOLUME", DbType.Decimal, objResumoDiario.Volume);
            db.AddInParameter(dbCommand, "@PRECO_ULTIMA_TRANSACAO", DbType.Decimal, objResumoDiario.PrecoUltimaTransacao);
            db.AddInParameter(dbCommand, "@VALOR_MAXIMO", DbType.Decimal, objResumoDiario.ValorMaximo);
            db.AddInParameter(dbCommand, "@VALOR_MINIMO", DbType.Decimal, objResumoDiario.ValorMinimo);
            db.AddInParameter(dbCommand, "@DT_DATA", DbType.DateTime, objResumoDiario.Data);
            db.AddInParameter(dbCommand, "@CEA_MOEDA", DbType.Int32, IntUtils.ToIntNullProc(objResumoDiario.Moeda.IdMoeda));
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objResumoDiario.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);
        }

        public void Remover(EntResumoDiario objResumoDiario, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_ResumoDiarioRemover");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddInParameter(dbCommand, "@CDA_RESUMO_DIARIO", DbType.Int32, objResumoDiario.IdResumoDiario);
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objResumoDiario.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);
        }

        public List<EntResumoDiario> ObterTodos(Int32 Moeda, DateTime DataInicio, DateTime DataFim, Int32 Status, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_ResumoDiarioSelecionarTodos");
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("Moeda"), DbType.Int32, IntUtils.ToIntNullProc(Moeda));
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("DataInicio"), DbType.DateTime, DataInicio);
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("DataFim"), DbType.DateTime, DataFim);
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
                    return new List<EntResumoDiario>();
                }
            }
        }

        private List<EntResumoDiario> Popular(DbDataReader dtrDados)
        {
            List<EntResumoDiario> listEntReturn = new List<EntResumoDiario>();
            EntResumoDiario entReturn;

            try
            {
                while (dtrDados.Read())
                {
                    entReturn = new EntResumoDiario();

                    entReturn.IdResumoDiario = ObjectUtils.ToInt(dtrDados["CDA_RESUMO_DIARIO"]);
                    entReturn.ResumoDiario = ObjectUtils.ToString(dtrDados["TX_RESUMO_DIARIO"]);
                    entReturn.Quantidade = ObjectUtils.ToDecimal(dtrDados["QUANTIDADE"]);
                    entReturn.MelhorCompra = ObjectUtils.ToDecimal(dtrDados["MELHOR_COMPRA"]);
                    entReturn.MelhorVenda = ObjectUtils.ToDecimal(dtrDados["MELHOR_VENDA"]);
                    entReturn.Volume = ObjectUtils.ToDecimal(dtrDados["VOLUME"]);
                    entReturn.PrecoUltimaTransacao = ObjectUtils.ToDecimal(dtrDados["PRECO_ULTIMA_TRANSACAO"]);
                    entReturn.ValorMaximo = ObjectUtils.ToDecimal(dtrDados["VALOR_MAXIMO"]);
                    entReturn.ValorMinimo = ObjectUtils.ToDecimal(dtrDados["VALOR_MINIMO"]);
                    entReturn.Data = ObjectUtils.ToDate(dtrDados["DT_DATA"]);
                    entReturn.Moeda.IdMoeda = ObjectUtils.ToInt(dtrDados["CEA_MOEDA"]);
                    entReturn.Moeda.Moeda = ObjectUtils.ToString(dtrDados["TX_MOEDA"]);
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

        public EntResumoDiario ObterPorId(Int32 IdResumoDiario, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_ResumoDiarioSelecionarPorIdResumoDiario");
            db.AddInParameter(dbCommand, "@IdResumoDiario", DbType.Int32, IdResumoDiario);
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            using (DbDataReader dtrDados = (System.Data.Common.DbDataReader)db.ExecuteReader(dbCommand, transaction))
            {
                if (dtrDados != null && dtrDados.HasRows)
                {
                    return this.Popular(dtrDados)[0];
                }
                else
                {
                    return new EntResumoDiario();
                }
            }
        }


    }
}