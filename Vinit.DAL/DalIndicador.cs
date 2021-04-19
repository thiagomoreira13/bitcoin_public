using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Vinit.BitCoiner.Ent;
using Vinit.Common;

namespace Vinit.BitCoiner.DAL
{
    public class DalIndicador
    {

        public EntIndicador Inserir(EntIndicador objIndicador, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_IndicadorInserir");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddOutParameter(dbCommand, "@CDA_INDICADOR", DbType.Int32, objIndicador.IdIndicador);
            db.AddInParameter(dbCommand, "@TX_INDICADOR", DbType.String, objIndicador.Indicador);
            db.AddInParameter(dbCommand, "@DT_DATA", DbType.DateTime, objIndicador.Data);
            db.AddInParameter(dbCommand, "@NU_VALOR", DbType.Decimal, objIndicador.Valor);
            db.AddInParameter(dbCommand, "@CEA_MOEDA", DbType.Int32, IntUtils.ToIntNullProc(objIndicador.Moeda.IdMoeda));
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objIndicador.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);

            objIndicador.IdIndicador = ObjectUtils.ToInt(db.GetParameterValue(dbCommand, "@CDA_INDICADOR"));

            return objIndicador;
        }

        public void Alterar(EntIndicador objIndicador, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_IndicadorAlterar");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddInParameter(dbCommand, "@CDA_INDICADOR", DbType.Int32, objIndicador.IdIndicador);
            db.AddInParameter(dbCommand, "@TX_INDICADOR", DbType.String, objIndicador.Indicador);
            db.AddInParameter(dbCommand, "@DT_DATA", DbType.DateTime, objIndicador.Data);
            db.AddInParameter(dbCommand, "@NU_VALOR", DbType.Decimal, objIndicador.Valor);
            db.AddInParameter(dbCommand, "@CEA_MOEDA", DbType.Int32, IntUtils.ToIntNullProc(objIndicador.Moeda.IdMoeda));
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objIndicador.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);
        }

        public void Remover(EntIndicador objIndicador, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_IndicadorRemover");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddInParameter(dbCommand, "@CDA_INDICADOR", DbType.Int32, objIndicador.IdIndicador);
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objIndicador.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);
        }

        public List<EntIndicador> ObterTodos(String Indicador, Int32 Moeda, DateTime DataInicio, DateTime DataFim, Int32 Status, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_IndicadorSelecionarTodos");
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("Indicador"), DbType.String, Indicador);
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("Moeda"), DbType.Int32, IntUtils.ToIntNullProc(Moeda));
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("Status"), DbType.Boolean, IntUtils.ToBooleanNullProc(Status));
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("DataInicio"), DbType.DateTime, DataInicio);
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("DataFim"), DbType.DateTime, DataFim);
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            using (DbDataReader dtrDados = (System.Data.Common.DbDataReader)db.ExecuteReader(dbCommand, transaction))
            {
                if (dtrDados != null && dtrDados.HasRows)
                {
                    return this.Popular(dtrDados);
                }
                else
                {
                    return new List<EntIndicador>();
                }
            }
        }

        private List<EntIndicador> Popular(DbDataReader dtrDados)
        {
            List<EntIndicador> listEntReturn = new List<EntIndicador>();
            EntIndicador entReturn;

            try
            {
                while (dtrDados.Read())
                {
                    entReturn = new EntIndicador();

                    entReturn.IdIndicador = ObjectUtils.ToInt(dtrDados["CDA_INDICADOR"]);
                    entReturn.Indicador = ObjectUtils.ToString(dtrDados["TX_INDICADOR"]);
                    entReturn.Data = ObjectUtils.ToDate(dtrDados["DT_DATA"]);
                    entReturn.Valor = ObjectUtils.ToDecimal(dtrDados["NU_VALOR"]);
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

        public EntIndicador ObterPorId(Int32 IdIndicador, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_IndicadorSelecionarPorIdIndicador");
            db.AddInParameter(dbCommand, "@IdIndicador", DbType.Int32, IdIndicador);
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            using (DbDataReader dtrDados = (System.Data.Common.DbDataReader)db.ExecuteReader(dbCommand, transaction))
            {
                if (dtrDados != null && dtrDados.HasRows)
                {
                    return this.Popular(dtrDados)[0];
                }
                else
                {
                    return new EntIndicador();
                }
            }
        }


    }
}