using Binance.Net;
using Binance.Net.Enums;
using Binance.Net.Objects.Spot;
using Binance.Net.Objects.Spot.MarketData;
using Binance.Net.Objects.Spot.SpotData;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Vinit.BitCoiner.BLL;
using Vinit.BitCoiner.Ent;
using Vinit.Common;

namespace RoboProcessador
{
    class RoboIntegracaoBitcoinMarket
    {
        public DateTime DataUltimaAtualizacao = DateTime.Now;
        BinanceClient client = null;

        public void AtualizaInformacoesResumo()
        {
            List<EntMoeda> listaMoedas = new BllMoeda().ObterTodos("", "", 1);

            foreach (EntMoeda objMoeda in listaMoedas)
            {
                if (objMoeda.OperacaoCompraAberta.IdOperacao > 0)
                {
                    objMoeda.OperacaoCompraAberta = new BllOperacao().ObterPorId(objMoeda.OperacaoCompraAberta.IdOperacao);
                }
                if (objMoeda.OperacaoVendaAberta.IdOperacao > 0)
                {
                    objMoeda.OperacaoVendaAberta = new BllOperacao().ObterPorId(objMoeda.OperacaoVendaAberta.IdOperacao);
                }
            }

            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(ConfigurationManager.AppSettings["BinanceClientKey"], ConfigurationManager.AppSettings["BinanceSecretKey"]),
                LogVerbosity = LogVerbosity.Warning,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            BinanceSocketClient.SetDefaultOptions(new BinanceSocketClientOptions()
            {
                ApiCredentials = new ApiCredentials(ConfigurationManager.AppSettings["BinanceClientKey"], ConfigurationManager.AppSettings["BinanceSecretKey"]),
                LogVerbosity = LogVerbosity.Warning,
                LogWriters = new List<TextWriter> { Console.Out }
            });

            client = new BinanceClient();

            /*foreach (EntMoeda moeda in listaMoedas)
            {
                if (moeda.IdMoeda == EntMoeda.MOEDA_ETH)
                {
                    MonitorarVenda(moeda, new EntCarteira());
                }
            }*/

            foreach (EntMoeda moeda in listaMoedas)
            {
                AtualizaInformacoesResumoMoeda(moeda);
                CalculaRsi(moeda);
            }

            while (true)
            {
                if (DateTime.Now.Subtract(DataUltimaAtualizacao).TotalMilliseconds / 1000 >= StringUtils.ToInt(ConfigurationManager.AppSettings["SegundosAtualizacaoResumo"]))
                {
                    DataUltimaAtualizacao = DateTime.Now;

                    foreach (EntMoeda moeda in listaMoedas)
                    {
                        AtualizaInformacoesResumoMoeda(moeda);
                        CalculaRsi(moeda);
                    }
                }
                System.Threading.Thread.Sleep(5 * 1000);
            }
        }

        private void Aviso(String codigoMoeda, String titulo, String detalhe, Boolean enviaEmail = true)
        {
            Console.WriteLine(codigoMoeda + " - " + titulo);
            if (detalhe != "")
            {
                Console.WriteLine(codigoMoeda + " - " + detalhe);
            }


            if (enviaEmail)
            {
                WebUtils.EnviaEmail("thiagomoreira13@gmail.com", titulo, detalhe);
            }
        }

        private void AtualizaInformacoesResumoMoeda(EntMoeda objMoeda)
        {
            try
            {
                // Spot.Market | Spot market info endpoints
                var resumo = client.Spot.Market.Get24HPrice(objMoeda.Codigo + EntMoeda.USDT);

                /*// Spot.Order | Spot order info endpoints
                client.Spot.Order.GetAllOrders("BTCUSDT");
                // Spot.System | Spot system endpoints
                client.Spot.System.GetExchangeInfo();
                // Spot.UserStream | Spot user stream endpoints. Should be used to subscribe to a user stream with the socket client
                client.Spot.UserStream.StartUserStream();*/

                //var result = client.Spot.Order.PlaceTestOrder("BNBBTC", OrderSide.Buy, OrderType.Limit, timeInForce: TimeInForce.GoodTillCancel, quantity: 1, price: 2);

                EntResumoDiario objResumo = new EntResumoDiario();
                objResumo.ResumoDiario = "Resumo " + objMoeda.Moeda;
                objResumo.Ativo = true;
                objResumo.Data = DateTime.Now;
                objResumo.Moeda.IdMoeda = objMoeda.IdMoeda;
                objResumo.PrecoUltimaTransacao = resumo.Data.LastPrice;
                objResumo.ValorMaximo = resumo.Data.HighPrice;
                objResumo.ValorMinimo = resumo.Data.LowPrice;
                objResumo.Volume = resumo.Data.BaseVolume;
                objResumo.Quantidade = resumo.Data.TotalTrades;
                objResumo.MelhorCompra = resumo.Data.AskPrice;
                objResumo.MelhorVenda = resumo.Data.BidPrice;
                new BllResumoDiario().Inserir(objResumo, EntUsuario.USUARIO_PADRAO);
            }
            catch (Exception ex)
            {
                Aviso(objMoeda.Codigo, "BITCOINER - ERRO ATUALIZAÇÃO RESUMO", "Houve um erro na atualização do resumo para " + objMoeda.Codigo + " em " + DateUtils.ToStringCompleto(DateTime.Now) + "\n\n" + ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        //formula: RSI = 100 – 100/ (1 + RS) RS = Average Gain of n days UP  / Average Loss of n days DOWN
        private void CalculaRsi(EntMoeda objMoeda)
        {
            try
            {
                List<BinanceSpotKline> klines = client.Spot.Market.GetKlines(objMoeda.Codigo + EntMoeda.USDT, Binance.Net.Enums.KlineInterval.OneMinute, null, null, 100).Data as List<BinanceSpotKline>;
                /*decimal rsi = calculateRSIValues(klines, 6, objMoeda.IdMoeda);
                Aviso("RSI 100 6: " + rsi, "", false);

                rsi = calculateRSIValues(klines, 8, objMoeda.IdMoeda);
                Aviso("RSI 100 8: " + rsi, "", false);

                rsi = calculateRSIValues(klines, 10, objMoeda.IdMoeda);
                Aviso("RSI 100 10: " + rsi, "", false);
                
                rsi = calculateRSIValues(klines, 5, objMoeda.IdMoeda);
                Aviso("RSI 100 5: " + rsi, "", false);*/

                decimal rsi = calculateRSIValues(klines, 14, objMoeda.IdMoeda);
                Aviso(objMoeda.Codigo, "RSI 100 14: " + rsi, "", false);
                //Console.WriteLine("---------");

                EntCarteira objCarteira = new BllCarteira().ObterTodos("", EntUsuario.USUARIO_PADRAO, objMoeda.IdMoeda, 1)[0];

                if (objMoeda.OperacaoCompraAberta.IdOperacao == 0 || (objMoeda.OperacaoCompraAberta.IdOperacao > 0 && objMoeda.OperacaoVendaAberta.IdOperacao == 0))
                {
                    CheckAndTriggerCreateOrder(rsi, objMoeda, objCarteira);
                }

                if (objMoeda.OperacaoVendaAberta.IdOperacao > 0)
                {
                    MonitorarVenda(objMoeda, objCarteira);
                }
                else if (objMoeda.OperacaoCompraAberta.IdOperacao > 0 && objMoeda.OperacaoCompraAberta.DataFimExecucao.Year < 2000)
                {
                    MonitorarCompra(objMoeda, objCarteira);
                }
            }
            catch (Exception ex)
            {
                Aviso(objMoeda.Codigo, "BITCOINER - ERRO CALCULO RSI", "Houve um erro no calculo do RSI " + objMoeda.Codigo + " em " + DateUtils.ToStringCompleto(DateTime.Now) + "\n\n" + ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        private decimal calculateRSIValues(List<BinanceSpotKline> prices, int periodLength, Int32 IdMoeda)
        {
            try
            {
                int lastBar = prices.Count - 1;
                int firstBar = lastBar - periodLength + 1;
                if (firstBar < 0)
                {
                    String msg = "Quote history length " + prices.Count + " is insufficient to calculate the indicator.";
                    throw new Exception(msg);
                }

                decimal aveGain = 0, aveLoss = 0;
                for (int bar = firstBar + 1; bar <= lastBar; bar++)
                {
                    decimal change = prices[bar].Close - prices[bar - 1].Close;
                    if (change >= 0)
                    {
                        aveGain += change;
                    }
                    else
                    {
                        aveLoss += change;
                    }
                }

                decimal rs = aveGain / Math.Abs(aveLoss);
                decimal value = 100 - 100 / (1 + rs);

                EntIndicador objIndicador = new EntIndicador();
                objIndicador.Ativo = true;
                objIndicador.Data = DateTime.Now;
                objIndicador.Indicador = "RSI";
                objIndicador.Moeda.IdMoeda = IdMoeda;
                objIndicador.Valor = value;
                new BllIndicador().Inserir(objIndicador, EntUsuario.USUARIO_PADRAO);

                return value;
            }
            catch { }
            return 50;
        }

        private void CheckAndTriggerCreateOrder(decimal rsi, EntMoeda objMoeda, EntCarteira objCarteira)
        {
            Decimal rsiSuperior = StringUtils.ToDecimal(ConfigurationManager.AppSettings["RsiLimiteSuperior"]);
            Decimal rsiInferior = StringUtils.ToDecimal(ConfigurationManager.AppSettings["RsiLimiteInferior"]);

            if ((rsi >= rsiSuperior || rsi <= rsiInferior) && objMoeda.OperacaoVendaAberta.IdOperacao == 0)
            {
                EntResumoDiario ultimoStatus = new BllResumoDiario().ObterTodos(objMoeda.IdMoeda, StringUtils.ToDate(DateUtils.ToString(DateTime.Now)), DateTime.Now, 1).OrderByDescending(x => x.Data).ToList()[0];

                Decimal valor = ultimoStatus.MelhorCompra;
                if (valor < ultimoStatus.MelhorVenda)
                {
                    valor = ultimoStatus.MelhorVenda;
                }
                if (valor < ultimoStatus.PrecoUltimaTransacao)
                {
                    valor = ultimoStatus.PrecoUltimaTransacao;
                }

                //RSI inferior - cria ordem de compra
                if (rsi <= rsiInferior && objCarteira.SaldoDolar > 10 && objMoeda.OperacaoCompraAberta.IdOperacao == 0)
                {
                    CriaOrdemCompra(objMoeda, objCarteira);
                }
                else if (rsi >= rsiSuperior && (objCarteira.SaldoBitcoin * valor) > 10 && objMoeda.OperacaoVendaAberta.IdOperacao == 0 && objMoeda.OperacaoCompraAberta.IdOperacao > 0)
                {
                    CriaOrdemVenda(objMoeda, objCarteira, ultimoStatus);
                }
            }

        }

        private void CriaOrdemCompra(EntMoeda objMoeda, EntCarteira objCarteira)
        {
            try
            {
                EntResumoDiario ultimoStatus = new BllResumoDiario().ObterTodos(objMoeda.IdMoeda, StringUtils.ToDate(DateUtils.ToString(DateTime.Now)), DateTime.Now, 1).OrderByDescending(x => x.Data).ToList()[0];

                Decimal valor = ultimoStatus.MelhorCompra;
                if (valor > ultimoStatus.MelhorVenda)
                {
                    valor = ultimoStatus.MelhorVenda;
                }
                if (valor > ultimoStatus.PrecoUltimaTransacao)
                {
                    valor = ultimoStatus.PrecoUltimaTransacao;
                }

                Decimal quantidade = objCarteira.SaldoDolar / valor;
                quantidade = DecimalUtils.ToDecimalRound(quantidade, objMoeda.CasasDepoisDaVirgula);

                if (StringUtils.ToBoolean(ConfigurationManager.AppSettings["teste"]))
                {
                    EntOperacao objOperacao = new EntOperacao();
                    objOperacao.Ativo = true;
                    objOperacao.Carteira = objCarteira;
                    objOperacao.DataCriacao = DateTime.Now;
                    objOperacao.DataInicioExecucao = DateTime.Now;
                    objOperacao.IsVenda = false;
                    objOperacao.Operacao = new Random().Next(10000, 99999).ToString();
                    objOperacao.ValorBitcoin = valor;
                    objOperacao.ValorDolar = quantidade * valor;
                    objOperacao.ValorReais = quantidade;
                    objOperacao = new BllOperacao().Inserir(objOperacao, EntUsuario.USUARIO_PADRAO);

                    objMoeda.OperacaoCompraAberta = objOperacao;
                    new BllMoeda().Alterar(objMoeda, EntUsuario.USUARIO_PADRAO);
                    Aviso(objMoeda.Codigo, "BITCOINER - ORDEM DE COMPRA", "Criada ordem de compra em " + DateUtils.ToStringCompleto(DateTime.Now) + "\n" + objMoeda.Codigo + "\nValor: US$ " + DecimalUtils.ToString_8(valor) + " | Quantidade: " + DecimalUtils.ToString_8(quantidade));
                }
                else
                {
                    var result = client.Spot.Order.PlaceOrder(objMoeda.Codigo + EntMoeda.USDT, OrderSide.Buy, OrderType.Market, quantity: quantidade);

                    if (result.Success)
                    {
                        EntOperacao objOperacao = new EntOperacao();
                        objOperacao.Ativo = true;
                        objOperacao.Carteira = objCarteira;
                        objOperacao.DataCriacao = DateTime.Now;
                        objOperacao.DataInicioExecucao = DateTime.Now;
                        objOperacao.IsVenda = false;
                        objOperacao.Operacao = result.Data.OrderId.ToString();
                        objOperacao.ValorBitcoin = valor;
                        objOperacao.ValorDolar = quantidade * valor;
                        objOperacao.ValorReais = quantidade;
                        objOperacao = new BllOperacao().Inserir(objOperacao, EntUsuario.USUARIO_PADRAO);

                        objMoeda.OperacaoCompraAberta = objOperacao;
                        new BllMoeda().Alterar(objMoeda, EntUsuario.USUARIO_PADRAO);

                        Aviso(objMoeda.Codigo, "BITCOINER - ORDEM DE COMPRA", "Criada ordem de compra em " + DateUtils.ToStringCompleto(DateTime.Now) + "\n" + objMoeda.Codigo + "\nValor: US$ " + DecimalUtils.ToString_8(valor) + " | Quantidade: " + DecimalUtils.ToString_8(quantidade));
                    }
                    else
                    {
                        Aviso(objMoeda.Codigo, "BITCOINER - ORDEM DE COMPRA", "ERRO Criada ordem de compra em " + DateUtils.ToStringCompleto(DateTime.Now) + "\n" + objMoeda.Codigo + "\nValor: US$ " + DecimalUtils.ToString_8(valor) + " | Quantidade: " + DecimalUtils.ToString_8(quantidade) +
                            "\n\n" + result.Error.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Aviso(objMoeda.Codigo, "BITCOINER - ERRO CRIAÇÃO ORDEM DE COMPRA", "Houve um erro na criação de ordem de compra " + objMoeda.Codigo + " em " + DateUtils.ToStringCompleto(DateTime.Now) + "\n\n" + ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        private void MonitorarCompra(EntMoeda objMoeda, EntCarteira objCarteira)
        {
            try
            {
                if (StringUtils.ToBoolean(ConfigurationManager.AppSettings["teste"]))
                {
                    Aviso(objMoeda.Codigo, "BITCOINER - ORDEM DE COMPRA CONCLUIDA", "Concluida ordem de compra em " + DateUtils.ToStringCompleto(DateTime.Now) + "\n" + objMoeda.Codigo + "\nValor: US$ " + DecimalUtils.ToString_8(objMoeda.OperacaoCompraAberta.ValorBitcoin) + " | Quantidade: " + DecimalUtils.ToString_8(objMoeda.OperacaoCompraAberta.ValorReais));

                    objMoeda.OperacaoCompraAberta.DataFimExecucao = DateTime.Now;
                    objMoeda.OperacaoCompraAberta.ValorBitcoin = objMoeda.OperacaoCompraAberta.ValorBitcoin;
                    new BllOperacao().Alterar(objMoeda.OperacaoCompraAberta, EntUsuario.USUARIO_PADRAO);

                    objCarteira.SaldoBitcoin = objCarteira.SaldoBitcoin + objMoeda.OperacaoCompraAberta.ValorReais;
                    objCarteira.SaldoDolar = objCarteira.SaldoDolar - objMoeda.OperacaoCompraAberta.ValorDolar;
                    new BllCarteira().Alterar(objCarteira, EntUsuario.USUARIO_PADRAO);

                    //objMoeda.OperacaoCompraAberta = new EntOperacao();
                    //new BllMoeda().Alterar(objMoeda, EntUsuario.USUARIO_PADRAO);
                }
                else
                {
                    WebCallResult<BinanceOrder> ordemTemp = client.Spot.Order.GetOrder(objMoeda.Codigo + EntMoeda.USDT, StringUtils.ToInt64(objMoeda.OperacaoCompraAberta.Operacao));
                    if (ordemTemp != null && ordemTemp.Data != null)
                    {
                        BinanceOrder ordem = ordemTemp.Data;
                        if (ordem.Status == OrderStatus.Filled)
                        {
                            Aviso(objMoeda.Codigo, "BITCOINER - ORDEM DE COMPRA CONCLUIDA", "Concluida ordem de compra em " + DateUtils.ToStringCompleto(DateTime.Now) + "\n" + objMoeda.Codigo + "\nValor: US$ " + DecimalUtils.ToString_8(ordem.Price) + " | Quantidade: " + DecimalUtils.ToString_8(ordem.QuantityFilled));

                            if (ordem.UpdateTime != null)
                            {
                                objMoeda.OperacaoCompraAberta.DataFimExecucao = ordem.UpdateTime.Value;
                            }
                            else
                            {
                                objMoeda.OperacaoCompraAberta.DataFimExecucao = DateTime.Now;
                            }
                            objMoeda.OperacaoCompraAberta.ValorBitcoin = ordem.Price;
                            new BllOperacao().Alterar(objMoeda.OperacaoCompraAberta, EntUsuario.USUARIO_PADRAO);

                            //objMoeda.OperacaoCompraAberta = new EntOperacao();
                            //new BllMoeda().Alterar(objMoeda, EntUsuario.USUARIO_PADRAO);

                            objCarteira.SaldoBitcoin = objCarteira.SaldoBitcoin + ordem.QuantityFilled;
                            objCarteira.SaldoDolar = objCarteira.SaldoDolar - ordem.QuoteQuantityFilled;
                            new BllCarteira().Alterar(objCarteira, EntUsuario.USUARIO_PADRAO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Aviso(objMoeda.Codigo, "BITCOINER - ERRO MONITORAMENTO COMPRA", "Houve um erro no monitoramento de compra " + objMoeda.Codigo + " em " + DateUtils.ToStringCompleto(DateTime.Now) + "\n\n" + ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        private void CriaOrdemVenda(EntMoeda objMoeda, EntCarteira objCarteira, EntResumoDiario ultimoStatus)
        {
            try
            {
                Decimal valor = ultimoStatus.MelhorCompra;
                if (valor < ultimoStatus.MelhorVenda)
                {
                    valor = ultimoStatus.MelhorVenda;
                }
                if (valor < ultimoStatus.PrecoUltimaTransacao)
                {
                    valor = ultimoStatus.PrecoUltimaTransacao;
                }

                if (valor < (objMoeda.OperacaoCompraAberta.ValorBitcoin * StringUtils.ToDecimal("1,005")))
                {
                    Aviso(objMoeda.Codigo, "BITCOINER - ORDEM DE VENDA NÃO CRIADA - VALOR MENOR QUE COMPRA", "Não criada ordem de venda em " + DateUtils.ToStringCompleto(DateTime.Now) + "\n" + objMoeda.Codigo + "\nValor: US$ " + DecimalUtils.ToString_8(valor), false);
                    return;
                }

                Decimal quantidade = objCarteira.SaldoBitcoin;
                quantidade = DecimalUtils.ToDecimalRound(quantidade, objMoeda.CasasDepoisDaVirgula);

                if (StringUtils.ToBoolean(ConfigurationManager.AppSettings["teste"]))
                {
                    EntOperacao objOperacao = new EntOperacao();
                    objOperacao.Ativo = true;
                    objOperacao.Carteira = objCarteira;
                    objOperacao.DataCriacao = DateTime.Now;
                    objOperacao.DataInicioExecucao = DateTime.Now;
                    objOperacao.IsVenda = true;
                    objOperacao.Operacao = new Random().Next(10000, 99999).ToString();
                    objOperacao.ValorBitcoin = valor;
                    objOperacao.ValorDolar = quantidade * valor;
                    objOperacao.ValorReais = quantidade;
                    objOperacao = new BllOperacao().Inserir(objOperacao, EntUsuario.USUARIO_PADRAO);

                    objMoeda.OperacaoVendaAberta = objOperacao;
                    new BllMoeda().Alterar(objMoeda, EntUsuario.USUARIO_PADRAO);

                    Aviso(objMoeda.Codigo, "BITCOINER - ORDEM DE VENDA", "Criada ordem de venda em " + DateUtils.ToStringCompleto(DateTime.Now) + "\n" + objMoeda.Codigo + "\nValor: US$ " + DecimalUtils.ToString_8(valor) + " | Quantidade: " + DecimalUtils.ToString_8(quantidade));
                }
                else
                {
                    var result = client.Spot.Order.PlaceOrder(objMoeda.Codigo + EntMoeda.USDT, OrderSide.Sell, OrderType.Market, quantity: quantidade);

                    if (result.Success)
                    {
                        EntOperacao objOperacao = new EntOperacao();
                        objOperacao.Ativo = true;
                        objOperacao.Carteira = objCarteira;
                        objOperacao.DataCriacao = DateTime.Now;
                        objOperacao.DataInicioExecucao = DateTime.Now;
                        objOperacao.IsVenda = true;
                        objOperacao.Operacao = result.Data.OrderId.ToString();
                        objOperacao.ValorBitcoin = valor;
                        objOperacao.ValorDolar = quantidade * valor;
                        objOperacao.ValorReais = quantidade;
                        objOperacao = new BllOperacao().Inserir(objOperacao, EntUsuario.USUARIO_PADRAO);

                        objMoeda.OperacaoVendaAberta = objOperacao;
                        new BllMoeda().Alterar(objMoeda, EntUsuario.USUARIO_PADRAO);

                        Aviso(objMoeda.Codigo, "BITCOINER - ORDEM DE VENDA", "Criada ordem de venda em " + DateUtils.ToStringCompleto(DateTime.Now) + "\n" + objMoeda.Codigo + "\nValor: US$ " + DecimalUtils.ToString_8(valor) + " | Quantidade: " + DecimalUtils.ToString_8(quantidade));
                    }
                    else
                    {
                        Aviso(objMoeda.Codigo, "BITCOINER - ORDEM DE VENDA", "ERRO Criada ordem de venda em " + DateUtils.ToStringCompleto(DateTime.Now) + "\n" + objMoeda.Codigo + "\nValor: US$ " + DecimalUtils.ToString_8(valor) + " | Quantidade: " + DecimalUtils.ToString_8(quantidade) +
                            "\n\n" + result.Error.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Aviso(objMoeda.Codigo, "BITCOINER - ERRO CRIAÇÃO ORDEM DE VENDA", "Houve um erro na criação de ordem de venda " + objMoeda.Codigo + " em " + DateUtils.ToStringCompleto(DateTime.Now) + "\n\n" + ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        private void MonitorarVenda(EntMoeda objMoeda, EntCarteira objCarteira)
        {
            try
            {
                if (StringUtils.ToBoolean(ConfigurationManager.AppSettings["teste"]))
                {
                    Aviso(objMoeda.Codigo, "BITCOINER - ORDEM DE VENDA CONCLUIDA", "Concluida ordem de venda em " + DateUtils.ToStringCompleto(DateTime.Now) + "\n" + objMoeda.Codigo + "\nValor: US$ " + DecimalUtils.ToString_8(objMoeda.OperacaoVendaAberta.ValorBitcoin) + " | Quantidade: " + DecimalUtils.ToString_8(objMoeda.OperacaoVendaAberta.ValorReais));

                    objMoeda.OperacaoVendaAberta.DataFimExecucao = DateTime.Now;
                    new BllOperacao().Alterar(objMoeda.OperacaoVendaAberta, EntUsuario.USUARIO_PADRAO);

                    objCarteira.SaldoBitcoin = objCarteira.SaldoBitcoin - objMoeda.OperacaoCompraAberta.ValorReais;
                    objCarteira.SaldoDolar = objCarteira.SaldoDolar + objMoeda.OperacaoCompraAberta.ValorReais * objMoeda.OperacaoVendaAberta.ValorBitcoin;
                    new BllCarteira().Alterar(objCarteira, EntUsuario.USUARIO_PADRAO);

                    objMoeda.OperacaoCompraAberta = new EntOperacao();
                    objMoeda.OperacaoVendaAberta = new EntOperacao();
                    new BllMoeda().Alterar(objMoeda, EntUsuario.USUARIO_PADRAO);
                }
                else
                {
                    WebCallResult<BinanceOrder> ordemTemp = client.Spot.Order.GetOrder(objMoeda.Codigo + EntMoeda.USDT, StringUtils.ToInt64(objMoeda.OperacaoVendaAberta.Operacao));
                    if (ordemTemp != null && ordemTemp.Data != null)
                    {
                        BinanceOrder ordem = ordemTemp.Data;
                        if (ordem.Status == OrderStatus.Filled)
                        {
                            Aviso(objMoeda.Codigo, "BITCOINER - ORDEM DE VENDA CONCLUIDA", "Concluida ordem de venda em " + DateUtils.ToStringCompleto(DateTime.Now) + "\n" + objMoeda.Codigo + "\nValor: US$ " + DecimalUtils.ToString_8(ordem.Price) + " | Quantidade: " + DecimalUtils.ToString_8(ordem.QuantityFilled));

                            if (ordem.UpdateTime != null)
                            {
                                objMoeda.OperacaoVendaAberta.DataFimExecucao = ordem.UpdateTime.Value;
                            }
                            else
                            {
                                objMoeda.OperacaoVendaAberta.DataFimExecucao = DateTime.Now;
                            }
                            objMoeda.OperacaoVendaAberta.ValorBitcoin = ordem.Price;
                            new BllOperacao().Alterar(objMoeda.OperacaoVendaAberta, EntUsuario.USUARIO_PADRAO);

                            objMoeda.OperacaoCompraAberta = new EntOperacao();
                            objMoeda.OperacaoVendaAberta = new EntOperacao();
                            new BllMoeda().Alterar(objMoeda, EntUsuario.USUARIO_PADRAO);

                            objCarteira.SaldoBitcoin = objCarteira.SaldoBitcoin - ordem.QuantityFilled;
                            objCarteira.SaldoDolar = objCarteira.SaldoDolar + ordem.QuoteQuantityFilled;
                            new BllCarteira().Alterar(objCarteira, EntUsuario.USUARIO_PADRAO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Aviso(objMoeda.Codigo, "BITCOINER - ERRO MONITORAMENTO VENDA", "Houve um erro no monitoramento de venda " + objMoeda.Codigo + " em " + DateUtils.ToStringCompleto(DateTime.Now) + "\n\n" + ex.Message + "\n\n" + ex.StackTrace);
            }
        }

    }
}