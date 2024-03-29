using System;
using Newtonsoft.Json;
using WindStock.JsonConverters;

namespace WindStock.Models
{
    public sealed class TwSourceStockData
    {
        [JsonProperty("c")]
        public string CommId { get; set; }

        [JsonProperty("ch")]
        public string Channel { get; set; }

        [JsonProperty("n")]
        public string Name { get; set; }

        [JsonProperty("nf")]
        public string FullName { get; set; }

        public string Time => DateTimeOffset.FromUnixTimeMilliseconds(TLong).ToString("yyyy-MM-dd HH:mm:ss");

        [JsonProperty("tlong")]
        public long TLong { get; set; }

        [JsonProperty("z")]
        public string LatestTradePrice { get; set; }

        [JsonProperty("tv")]
        public string TradeVolume { get; set; }

        [JsonProperty("v")]
        public string AccumulateTradeVolume { get; set; }

        [JsonProperty("b")]
        [JsonConverter(typeof(StringSplitConverter<decimal>), "_")]
        public decimal[] BestBidPrice { get; set; }

        [JsonProperty("g")]
        [JsonConverter(typeof(StringSplitConverter<decimal>), "_")]
        public decimal[] BestBidVolume { get; set; }

        [JsonProperty("a")]
        [JsonConverter(typeof(StringSplitConverter<decimal>), "_")]
        public decimal[] BestAskPrice { get; set; }

        [JsonProperty("f")]
        [JsonConverter(typeof(StringSplitConverter<decimal>), "_")]
        public decimal[] BestAskVolume { get; set; }

        [JsonProperty("o")]
        public decimal Open { get; set; }

        [JsonProperty("h")]
        public decimal High { get; set; }

        [JsonProperty("l")]
        public decimal Low { get; set; }
    }
}
