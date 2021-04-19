using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Vinit.BitCoiner.Ent;
using Vinit.Common;

namespace Vinit.BitCoiner.DAL
{
    public class DalCarteira
    {

        public EntCarteira Inserir(EntCarteira objCarteira, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_CarteiraInserir");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddOutParameter(dbCommand, "@CDA_CARTEIRA", DbType.Int32, objCarteira.IdCarteira);
            db.AddInParameter(dbCommand, "@TX_CARTEIRA", DbType.String, objCarteira.Carteira);
            db.AddInParameter(dbCommand, "@CEA_USUARIO", DbType.Int32, IntUtils.ToIntNullProc(objCarteira.Usuario.IdUsuario));
            db.AddInParameter(dbCommand, "@SALDO_REAIS", DbType.Decimal, objCarteira.SaldoReais);
            db.AddInParameter(dbCommand, "@SALDO_DOLAR", DbType.Decimal, objCarteira.SaldoDolar);
            db.AddInParameter(dbCommand, "@SALDO_BITCOIN", DbType.Decimal, objCarteira.SaldoBitcoin);
            db.AddInParameter(dbCommand, "@SALDO_REAIS2", DbType.Decimal, objCarteira.SaldoReais2);
            db.AddInParameter(dbCommand, "@SALDO_DOLAR2", DbType.Decimal, objCarteira.SaldoDolar2);
            db.AddInParameter(dbCommand, "@SALDO_BITCOIN2", DbType.Decimal, objCarteira.SaldoBitcoin2);
            db.AddInParameter(dbCommand, "@PERCENTUAL_TAXA_COMPRA", DbType.Decimal, objCarteira.PercentualTaxaCompra);
            db.AddInParameter(dbCommand, "@PERCENTUAL_TAXA_VENDA", DbType.Decimal, objCarteira.PercentualTaxaVenda);
            db.AddInParameter(dbCommand, "@PERCENTUAL_LUCRO", DbType.Decimal, objCarteira.PercentualLucro);
            db.AddInParameter(dbCommand, "@CEA_MOEDA", DbType.Int32, IntUtils.ToIntNullProc(objCarteira.Moeda.IdMoeda));
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objCarteira.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);

            objCarteira.IdCarteira = ObjectUtils.ToInt(db.GetParameterValue(dbCommand, "@CDA_CARTEIRA"));

            return objCarteira;
        }

        public void Alterar(EntCarteira objCarteira, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_CarteiraAlterar");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddInParameter(dbCommand, "@CDA_CARTEIRA", DbType.Int32, objCarteira.IdCarteira);
            db.AddInParameter(dbCommand, "@TX_CARTEIRA", DbType.String, objCarteira.Carteira);
            db.AddInParameter(dbCommand, "@CEA_USUARIO", DbType.Int32, IntUtils.ToIntNullProc(objCarteira.Usuario.IdUsuario));
            db.AddInParameter(dbCommand, "@SALDO_REAIS", DbType.Decimal, objCarteira.SaldoReais);
            db.AddInParameter(dbCommand, "@SALDO_DOLAR", DbType.Decimal, objCarteira.SaldoDolar);
            db.AddInParameter(dbCommand, "@SALDO_BITCOIN", DbType.Decimal, objCarteira.SaldoBitcoin);
            db.AddInParameter(dbCommand, "@SALDO_REAIS2", DbType.Decimal, objCarteira.SaldoReais2);
            db.AddInParameter(dbCommand, "@SALDO_DOLAR2", DbType.Decimal, objCarteira.SaldoDolar2);
            db.AddInParameter(dbCommand, "@SALDO_BITCOIN2", DbType.Decimal, objCarteira.SaldoBitcoin2);
            db.AddInParameter(dbCommand, "@PERCENTUAL_TAXA_COMPRA", DbType.Decimal, objCarteira.PercentualTaxaCompra);
            db.AddInParameter(dbCommand, "@PERCENTUAL_TAXA_VENDA", DbType.Decimal, objCarteira.PercentualTaxaVenda);
            db.AddInParameter(dbCommand, "@PERCENTUAL_LUCRO", DbType.Decimal, objCarteira.PercentualLucro);
            db.AddInParameter(dbCommand, "@CEA_MOEDA", DbType.Int32, IntUtils.ToIntNullProc(objCarteira.Moeda.IdMoeda));
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objCarteira.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);
        }

        public void Remover(EntCarteira objCarteira, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_CarteiraRemover");
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            db.AddInParameter(dbCommand, "@CDA_CARTEIRA", DbType.Int32, objCarteira.IdCarteira);
            db.AddInParameter(dbCommand, "@FL_ATIVO", DbType.Boolean, objCarteira.Ativo);

            db.ExecuteNonQuery(dbCommand, transaction);
        }

        public List<EntCarteira> ObterTodos(String Carteira, Int32 Usuario, Int32 Moeda, Int32 Status, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_CarteiraSelecionarTodos");
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("Carteira"), DbType.String, Carteira);
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("Usuario"), DbType.Int32, IntUtils.ToIntNullProc(Usuario));
            db.AddInParameter(dbCommand, StringUtils.TrataParametroProc("Moeda"), DbType.Int32, IntUtils.ToIntNullProc(Moeda));
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
                    return new List<EntCarteira>();
                }
            }
        }

        private List<EntCarteira> Popular(DbDataReader dtrDados)
        {
            List<EntCarteira> listEntReturn = new List<EntCarteira>();
            EntCarteira entReturn;

            try
            {
                while (dtrDados.Read())
                {
                    entReturn = new EntCarteira();

                    entReturn.IdCarteira = ObjectUtils.ToInt(dtrDados["CDA_CARTEIRA"]);
                    entReturn.Carteira = ObjectUtils.ToString(dtrDados["TX_CARTEIRA"]);
                    entReturn.Usuario.IdUsuario = ObjectUtils.ToInt(dtrDados["CEA_USUARIO"]);
                    entReturn.Usuario.Usuario = ObjectUtils.ToString(dtrDados["TX_USUARIO"]);
                    entReturn.SaldoReais = ObjectUtils.ToDecimal(dtrDados["SALDO_REAIS"]);
                    entReturn.SaldoDolar = ObjectUtils.ToDecimal(dtrDados["SALDO_DOLAR"]);
                    entReturn.SaldoBitcoin = ObjectUtils.ToDecimal(dtrDados["SALDO_BITCOIN"]);
                    entReturn.SaldoReais2 = ObjectUtils.ToDecimal(dtrDados["SALDO_REAIS2"]);
                    entReturn.SaldoDolar2 = ObjectUtils.ToDecimal(dtrDados["SALDO_DOLAR2"]);
                    entReturn.SaldoBitcoin2 = ObjectUtils.ToDecimal(dtrDados["SALDO_BITCOIN2"]);
                    entReturn.PercentualTaxaCompra = ObjectUtils.ToDecimal(dtrDados["PERCENTUAL_TAXA_COMPRA"]);
                    entReturn.PercentualTaxaVenda = ObjectUtils.ToDecimal(dtrDados["PERCENTUAL_TAXA_VENDA"]);
                    entReturn.PercentualLucro = ObjectUtils.ToDecimal(dtrDados["PERCENTUAL_LUCRO"]);
                    entReturn.Moeda.IdMoeda = ObjectUtils.ToInt(dtrDados["CEA_MOEDA"]);
                    entReturn.Moeda.Moeda = ObjectUtils.ToString(dtrDados["TX_MOEDA"]);
                    entReturn.Moeda.CasasDepoisDaVirgula = ObjectUtils.ToInt(dtrDados["NU_CASAS_APOS_VIRGULA"]);
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

        public EntCarteira ObterPorId(Int32 IdCarteira, DbTransaction transaction, Database db)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("STP_CarteiraSelecionarPorIdCarteira");
            db.AddInParameter(dbCommand, "@IdCarteira", DbType.Int32, IdCarteira);
            dbCommand.CommandTimeout = BdConfig.CommmandTimeout;

            using (DbDataReader dtrDados = (System.Data.Common.DbDataReader)db.ExecuteReader(dbCommand, transaction))
            {
                if (dtrDados != null && dtrDados.HasRows)
                {
                    return this.Popular(dtrDados)[0];
                }
                else
                {
                    return new EntCarteira();
                }
            }
        }


    }
}