using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WindStock;
using WindStock.Models;

namespace WinStock.Example
{
    class Program
    {
        private static TwStockExchange _exchange;
        private static CancellationTokenSource _cts = new CancellationTokenSource();

        static void Main(string[] args)
        {
            var crawler = new TwStockCrawler();

            var data = crawler.GetAsync("2330", TradeType.TSE).Result;

            Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));

            _exchange = new TwStockExchange(crawler);
            _exchange.Subscribe("2330", TradeType.TSE, OnMessage);

            Console.CancelKeyPress += ConsoleOnCancelKeyPress;

            SpinWait.SpinUntil(() => _cts.IsCancellationRequested);
        }

        private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            _exchange.Dispose();
            _cts.Cancel(false);
        }

        private static void OnMessage(TwSourceStockData data)
        {
            Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
        }
    }
}
