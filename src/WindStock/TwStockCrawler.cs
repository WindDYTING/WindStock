using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using WindStock.Models;

namespace WindStock
{
    public class TwStockCrawler : ICrawler<TwSourceStockData>, IDisposable
    {
        public const string SessionUrl = "https://mis.twse.com.tw/stock/index.jsp";
        public const string StockInfoUrl = "https://mis.twse.com.tw/stock/api/getStockInfo.jsp?ex_ch={0}&json=1&delay=0_&={1}";
        private RestClient Web { get; } = RestClientFactory.Get(typeof(TwStockCrawler));

        public TwStockCrawler()
        {
            TwCommodities.Load();
        }

        public async Task<TwSourceStockData> GetAsync(string commId, TradeType tradeType)
        {
            Preconditions.NotNullOrEmpty(commId);

            var raw = await GetRawDataAsync(commId, tradeType);

            var value = raw["msgArray"][0].ToString();
            var msgArray = JsonConvert.DeserializeObject<TwSourceStockData>(value);

            return msgArray;
        }

        public TwSourceStockData Get(string commId)
        {
            return Get(commId, TradeType.TSE);
        }

        public TwSourceStockData Get(string commId, TradeType tradeType)
        {
            Preconditions.NotNullOrEmpty(commId);

            var raw = GetRawData(commId, tradeType);

            var value = raw["msgArray"][0].ToString();
            var msgArray = JsonConvert.DeserializeObject<TwSourceStockData>(value);

            return msgArray;
        }

        public Task<TwSourceStockData> GetAsync(string commId)
        {
            return GetAsync(commId, TradeType.TSE);
        }

        private IDictionary<string, JToken> GetRawData(string commId, TradeType tradeType)
        {
            if (!TwCommodities.AllStocks.ContainsKey(commId))
            {
                throw new ArgumentException("commId does not exist", commId);
            }

            try
            {
                var req = new RestRequest(SessionUrl);
                Web.Get(req);
            }
            catch
            {
                // ban
            }

            var req2 = new RestRequest(string.Format(StockInfoUrl, $"{tradeType.ToString().ToLower()}_{commId}.tw", DateTimeOffset.Now.ToUnixTimeMilliseconds()));
            var resp = Web.Get(req2);

            return JsonConvert.DeserializeObject<IDictionary<string, JToken>>(resp.Content);
        }

        private async Task<IDictionary<string, JToken>> GetRawDataAsync(string commId, TradeType tradeType)
        {
            if (!TwCommodities.AllStocks.ContainsKey(commId))
            {
                throw new ArgumentException("commId does not exist", commId);
            }

            try
            {
                var req = new RestRequest(SessionUrl);
                Web.Get(req);
            }
            catch
            {
                // ban
            }

            var req2 = new RestRequest(string.Format(StockInfoUrl, $"{tradeType.ToString().ToLower()}_{commId}.tw", DateTimeOffset.Now.ToUnixTimeMilliseconds()));
            var resp = await Web.GetAsync(req2);

            return JsonConvert.DeserializeObject<IDictionary<string, JToken>>(resp.Content);
        }

        public void Dispose()
        {
            RestClientFactory.Returns(typeof(TwSourceStockData));
        }
    }
}
