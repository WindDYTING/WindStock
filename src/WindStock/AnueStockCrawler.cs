using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using WindStock.Models;

namespace WindStock
{
    public class AnueStockCrawler : ICrawler<AnueSourceStockData>
    {
        private const string RequestUrl = "https://ws.api.cnyes.com/ws/api/v1/quote/quotes/{0}?column=C,K,E,KEY,M,AI";
        private const string CheckCommIdExistUrl = "https://ws.api.cnyes.com/internal/ws/api/v1/checkTWStock/{0}";

        private RestClient Web { get; } = RestClientFactory.Get(typeof(AnueStockCrawler));

        public AnueStockCrawler()
        {
            TwCommodities.Load();
        }

        public async Task<AnueSourceStockData> GetAsync(string commId, TradeType tradeType)
        {
            Preconditions.NotNullOrEmpty(commId);

            if (tradeType == TradeType.OTC) throw new NotSupportedException();

            var raw = await GetRawDataAsync(commId, tradeType);
            var statusCode = raw["statusCode"].ToObject<int>();

            if (statusCode is >= 200 and <= 299)
            {
                var data = raw["data"][0].ToString();
                return JsonConvert.DeserializeObject<AnueSourceStockData>(data);
            }

            var message = raw["message"].ToString();
            _ = Enum.TryParse(typeof(HttpStatusCode), statusCode.ToString(), out var @enum);
            throw new HttpRequestException(message, null, (HttpStatusCode)@enum);
        }

        public Task<AnueSourceStockData> GetAsync(string commId)
        {
            return GetAsync(commId, TradeType.TSE);
        }

        public AnueSourceStockData Get(string commId)
        {
            return Get(commId, TradeType.TSE);
        }

        public AnueSourceStockData Get(string commId, TradeType tradeType)
        {
            Preconditions.NotNullOrEmpty(commId);

            if (tradeType == TradeType.OTC) throw new NotSupportedException();

            var raw = GetRawData(commId, tradeType);
            var statusCode = raw["statusCode"].ToObject<int>();

            if (statusCode is >= 200 and <= 299)
            {
                var data = raw["data"][0].ToString();
                return JsonConvert.DeserializeObject<AnueSourceStockData>(data);
            }

            var message = raw["message"].ToString();
            _ = Enum.TryParse(typeof(HttpStatusCode), statusCode.ToString(), out var @enum);
            throw new HttpRequestException(message, null, (HttpStatusCode)@enum);
        }

        private bool IsCommIdExist(string commId)
        {
            var req = new RestRequest(string.Format(CheckCommIdExistUrl, commId));
            var resp = Web.Get(req);

            return resp.IsSuccessStatusCode;
        }

        private IDictionary<string, JToken> GetRawData(string commId, TradeType tradeType)
        {
            if (!IsCommIdExist(commId))
            {
                throw new ArgumentException("commId does not exist", commId);
            }

            var req2 = new RestRequest(string.Format(RequestUrl, $"TWS:{commId}:STOCK", DateTimeOffset.Now.ToUnixTimeMilliseconds()));
            var resp = Web.Get(req2);

            return JsonConvert.DeserializeObject<IDictionary<string, JToken>>(resp.Content);
        }

        private async Task<IDictionary<string, JToken>> GetRawDataAsync(string commId, TradeType tradeType)
        {
            if (!IsCommIdExist(commId))
            {
                throw new ArgumentException("commId does not exist", commId);
            }

            var req2 = new RestRequest(string.Format(RequestUrl, $"TWS:{commId}:STOCK", DateTimeOffset.Now.ToUnixTimeMilliseconds()));
            var resp = await Web.GetAsync(req2);

            return JsonConvert.DeserializeObject<IDictionary<string, JToken>>(resp.Content);
        }

        public void Dispose()
        {
            RestClientFactory.Returns(typeof(AnueStockCrawler));
        }
    }
}
