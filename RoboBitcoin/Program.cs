using RoboProcessador;
using System.Threading;

namespace RoboBitcoin
{
    class Program
    {
        static void Main(string[] args)
        {
            RoboIntegracaoBitcoinMarket worker = new RoboIntegracaoBitcoinMarket();
            Thread threadResumo = new Thread(worker.AtualizaInformacoesResumo);
            threadResumo.Start();

        }
    }
}
