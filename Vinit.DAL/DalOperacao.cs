using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Vinit.BitCoiner.Ent;
using Vinit.Common;

namespace Vinit.BitCoiner.DAL
{
    public class DalOperacao
    {

        public EntOperacao Inserir(EntOperacao objOperacao, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_OperacaoInserir");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddOutParameter(dbCommand, "@CDA_OPERACAO", DbType.Int32, objOperacao.IdOperacao);
            db.AddInParameter(dbCommand, "@TX_OPERACAO", DbType.String, objOperacao.Operacao);
            db.AddInParameter(dbCommand, "@DT_DATA_CRIACAO", DbType.DateTime, objOperacao.DataCriacao);
            db.AddInParameter(dbCommand, "@DT_DATA_INICIO_EXECUCAO", DbType.DateTime, DateUtils.ToDateObject(objOperacao.DataInicioExecucao));
            db.AddInParameter(dbCommand, "@DT_DATA_FIM_EXECUCAO", DbType.DateTime, DateUtils.ToDateObject(objOperacao.DataFimExecucao));
            db.AddInParameter(dbCommand, "@VALOR_REAIS", DbType.Decimal, objOperacao.ValorReais);
            db.AddInParameter(dbCommand, "@VALOR_DOLAR", DbType.Decimal, objOperacao.ValorDolar);
            db.AddInParameter(dbCommand, "@VALOR_BITCOIN", DbType.Decimal, objOperacao.ValorBitcoin);
            db.AddInParameter(dbCommand, "@CEA_CARTEIRA", DbType.Int32, IntUtils.ToIntNullProc(objOperacao.Carteira.IdCarteira));
            db.AddInParameter(dbCommand, "@FL_IS_VENDA", DbType.Boolean, objOperacao.IsVenda);
            db.AddInParameter(dbCommand, "@TAXA", DbType.Decimal, objOperacao.Taxa);
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objOperacao.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);

            objOperacao.IdOperacao = ObjectUtils.ToInt(db.GetParameterValue(dbCommand, "@CDA_OPERACAO"));

            return objOperacao;
        }

        public void Alterar(EntOperacao objOperacao, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_OperacaoAlterar");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddInParameter(dbCommand, "@CDA_OPERACAO", DbType.Int32, objOperacao.IdOperacao);
            db.AddInParameter(dbCommand, "@TX_OPERACAO", DbType.String, objOperacao.Operacao);
            db.AddInParameter(dbCommand, "@DT_DATA_CRIACAO", DbType.DateTime, objOperacao.DataCriacao);
            db.AddInParameter(dbCommand, "@DT_DATA_INICIO_EXECUCAO", DbType.DateTime, DateUtils.ToDateObject(objOperacao.DataInicioExecucao));
            db.AddInParameter(dbCommand, "@DT_DATA_FIM_EXECUCAO", DbType.DateTime, DateUtils.ToDateObject(objOperacao.DataFimExecucao));
            db.AddInParameter(dbCommand, "@VALOR_REAIS", DbType.Decimal, objOperacao.ValorReais);
            db.AddInParameter(dbCommand, "@VALOR_DOLAR", DbType.Decimal, objOperacao.ValorDolar);
            db.AddInParameter(dbCommand, "@VALOR_BITCOIN", DbType.Decimal, objOperacao.ValorBitcoin);
            db.AddInParameter(dbCommand, "@CEA_CARTEIRA", DbType.Int32, IntUtils.ToIntNullProc(objOperacao.Carteira.IdCarteira));
            db.AddInParameter(dbCommand, "@FL_IS_VENDA", DbType.Boolean, objOperacao.IsVenda);
            db.AddInParameter(dbCommand, "@TAXA", DbType.Decimal, objOperacao.Taxa);
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objOperacao.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);
        }

        public void Remover(EntOperacao objOperacao, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_OperacaoRemover");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddInParameter(dbCommand, "@CDA_OPERACAO", DbType.Int32, objOperacao.IdOperacao);
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objOperacao.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);
        }

        public List<EntOperacao> ObterTodos(String Operacao, Int32 Carteira, Int32 IsVenda, Int32 Status, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_OperacaoSelecionarTodos");
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("Operacao"), DbType.String, Operacao);
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("Carteira"), DbType.Int32, IntUtils.ToIntNullProc(Carteira));
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("IsVenda"), DbType.Int32, IntUtils.ToBooleanNullProc(IsVenda));
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
                    return new List<EntOperacao>();
                }
            }
        }

        private List<EntOperacao> Popular(DbDataReader dtrDados)
        {
            List<EntOperacao> listEntReturn = new List<EntOperacao>();
            EntOperacao entReturn;

            try
            {
                while (dtrDados.Read())
                {
                    entReturn = new EntOperacao();

                    entReturn.IdOperacao = ObjectUtils.ToInt(dtrDados["CDA_OPERACAO"]);
                    entReturn.Operacao = ObjectUtils.ToString(dtrDados["TX_OPERACAO"]);
                    entReturn.DataCriacao = ObjectUtils.ToDate(dtrDados["DT_DATA_CRIACAO"]);
                    entReturn.DataInicioExecucao = ObjectUtils.ToDate(dtrDados["DT_DATA_INICIO_EXECUCAO"]);
                    entReturn.DataFimExecucao = ObjectUtils.ToDate(dtrDados["DT_DATA_FIM_EXECUCAO"]);
                    entReturn.ValorReais = ObjectUtils.ToDecimal(dtrDados["VALOR_REAIS"]);
                    entReturn.ValorDolar = ObjectUtils.ToDecimal(dtrDados["VALOR_DOLAR"]);
                    entReturn.ValorBitcoin = ObjectUtils.ToDecimal(dtrDados["VALOR_BITCOIN"]);
                    entReturn.Carteira.IdCarteira = ObjectUtils.ToInt(dtrDados["CEA_CARTEIRA"]);
                    entReturn.Carteira.Carteira = ObjectUtils.ToString(dtrDados["TX_CARTEIRA"]);
                    entReturn.Carteira.Usuario.IdUsuario = ObjectUtils.ToInt(dtrDados["CEA_USUARIO"]);
                    entReturn.Carteira.Usuario.Usuario = ObjectUtils.ToString(dtrDados["TX_USUARIO"]);
                    entReturn.Carteira.Moeda.IdMoeda = ObjectUtils.ToInt(dtrDados["CEA_MOEDA"]);
                    entReturn.Carteira.Moeda.Moeda = ObjectUtils.ToString(dtrDados["TX_MOEDA"]);
                    entReturn.Carteira.Moeda.Codigo = ObjectUtils.ToString(dtrDados["TX_MOEDA_CODIGO"]);
                    entReturn.Carteira.Moeda.CasasDepoisDaVirgula = ObjectUtils.ToInt(dtrDados["NU_CASAS_APOS_VIRGULA"]);
                    entReturn.IsVenda = ObjectUtils.ToBoolean(dtrDados["FL_IS_VENDA"]);
                    entReturn.Taxa = ObjectUtils.ToDecimal(dtrDados["TAXA"]);
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

        public EntOperacao ObterPorId(Int32 IdOperacao, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_OperacaoSelecionarPorIdOperacao");
            db.AddInParameter(dbCommand, "@IdOperacao", DbType.Int32, IdOperacao);
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            using (DbDataReader dtrDados = (System.Data.Common.DbDataReader)db.ExecuteReader(dbCommand, transaction))
            {
                if (dtrDados != null && dtrDados.HasRows)
                {
                    return this.Popular(dtrDados)[0];
                }
                else
                {
                    return new EntOperacao();
                }
            }
        }


    }
}

