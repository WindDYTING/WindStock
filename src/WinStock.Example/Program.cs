using System;
using Newtonsoft.Json;
using WindStock;
using WindStock.Models;

namespace WinStock.Example
{
    public class Program
    {
        static void Main(string[] args)
        {
            TwStockExample();

            AnueExample();

            Console.Read();
        }

        private static void TwStockExample()
        {
            var twStockCrawler = new TwStockCrawler();
            var twStock = twStockCrawler.Get("2330", TradeType.TSE);
            Console.Write(JsonConvert.SerializeObject(twStock, Formatting.Indented));

            var twStockExchange = new TwStockExchange(twStockCrawler);
            twStockExchange.Subscribe("2330", TradeType.TSE, OnMessage);
        }

        private static void AnueExample()
        {
            var anueStockCrawler = new AnueStockCrawler();

            var anueData = anueStockCrawler.Get("2330", TradeType.TSE);
            Console.Write(JsonConvert.SerializeObject(anueData, Formatting.Indented));

            var anueStockExchange = new AnueStockExchange(anueStockCrawler);
            anueStockExchange.Subscribe("2330", TradeType.TSE, OnMessage);
        }

        private static void OnMessage(TwSourceStockData obj)
        {
            Console.Write(JsonConvert.SerializeObject(obj, Formatting.Indented));
        }

        private static void OnMessage(AnueSourceStockData data)
        {
            Console.Write(JsonConvert.SerializeObject(data, Formatting.Indented));
        }
    }
}
