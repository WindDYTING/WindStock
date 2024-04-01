using System;
using Newtonsoft.Json;

namespace WindStock.Models
{
    public class AnueSourceStockData
    {
        [JsonProperty("0")]
        public string Code { get; set; }

        [JsonProperty("800013")]
        public string Symbol { get; set; }

        [JsonProperty("200193")]
        public string YearHighestDate { get; set; }

        [JsonProperty("800014")]
        public string ExchangeName { get; set; }

        [JsonProperty("200192")]
        public decimal YearHigh { get; set; }

        [JsonProperty("200196")]
        public decimal YearLow { get; set; }

        [JsonProperty("34")]
        public decimal YearEPS { get; set; }

        [JsonProperty("200223")]
        public decimal EPS { get; set; }

        [JsonProperty("6")]
        public decimal Price { get; set; }

        [JsonProperty("200197")]
        public string YearLowestDate { get; set; }

        [JsonProperty("11")]
        public decimal Change { get; set; }

        [JsonProperty("12")]
        public decimal HighPrice { get; set; }

        [JsonProperty("13")]
        public decimal LowPrice { get; set; }

        [JsonProperty("21")]
        public decimal LastClose { get; set; }

        [JsonProperty("800002")]
        public int IsOpening { get; set; }

        [JsonProperty("800001")]
        public long Volume { get; set; }

        [JsonProperty("19")]
        public decimal OpenPrice { get; set; }

        [JsonProperty("200219")]
        public decimal ProfitMargin { get; set; }

        [JsonProperty("200216")]
        public decimal BookValuePerShare { get; set; }

        [JsonProperty("200222")]
        public string IndustryMarket { get; set; }

        [JsonProperty("200087")]
        public string IndustryCategories { get; set; }

        [JsonProperty("200221")]
        public decimal OperatingMargin { get; set; }

        [JsonProperty("200220")]
        public decimal GrossMargin { get; set; }

        [JsonProperty("200226")]
        public decimal Capital { get; set; }

        [JsonProperty("200225")]
        public decimal Yield { get; set; }

        [JsonProperty("200224")]
        public decimal Dividend { get; set; }

        [JsonProperty("200096")]
        public string IndustryIndex { get; set; }

        [JsonProperty("36")]
        public decimal PE { get; set; }

        [JsonProperty("800041")]
        public int MarketStatus { get; set; }

        [JsonProperty("436")]
        public decimal BestBid1 { get; set; }

        [JsonProperty("437")]
        public decimal BestBid2 { get; set; }

        [JsonProperty("438")]
        public decimal BestBid3 { get; set; }

        [JsonProperty("439")]
        public decimal BestBid4 { get; set; }

        [JsonProperty("440")]
        public decimal BestBid5 { get; set; }

        [JsonProperty("56")]
        public decimal ChangePercent { get; set; }

        [JsonProperty("441")]
        public decimal BestAsk1 { get; set; }

        [JsonProperty("442")]
        public decimal BestAsk2 { get; set; }

        [JsonProperty("443")]
        public decimal BestAsk3 { get; set; }

        [JsonProperty("444")]
        public decimal BestAsk4 { get; set; }

        [JsonProperty("445")]
        public decimal BestAsk5 { get; set; }

        [JsonProperty("200127")]
        public decimal TurnoverPercentage { get; set; }

        [JsonProperty("200124")]
        public decimal Amplitude { get; set; }

        [JsonProperty("200007")]
        public long TradeTime { get; set; }

        public DateTimeOffset TradeDateTime => DateTimeOffset.FromUnixTimeSeconds(TradeTime);

        [JsonProperty("200011")]
        public string Exchange { get; set; }

        [JsonProperty("200010")]
        public string CommId { get; set; }

        [JsonProperty("200009")]
        public string DisplayName { get; set; }

        [JsonProperty("75")]
        public decimal LimitUp { get; set; }

        [JsonProperty("76")]
        public decimal LimitDown { get; set; }

        [JsonProperty("3404")]
        public decimal AveragePrice { get; set; }

        [JsonProperty("200014")]
        public double BestBidVolume1 { get; set; }
        
        [JsonProperty("200015")]
        public double BestBidVolume2 { get; set; }

        [JsonProperty("200016")]
        public double BestBidVolume3 { get; set; }

        [JsonProperty("200017")]
        public double BestBidVolume4 { get; set; }

        [JsonProperty("200018")]
        public double BestBidVolume5 { get; set; }

        [JsonProperty("200019")]
        public double BestAskVolume1 { get; set; }

        [JsonProperty("200020")]
        public double BestAskVolume2 { get; set; }

        [JsonProperty("200021")]
        public double BestAskVolume3 { get; set; }

        [JsonProperty("200022")]
        public double BestAskVolume4 { get; set; }

        [JsonProperty("200023")]
        public double BestAskVolume5 { get; set; }

        [JsonProperty("200144")]
        public decimal LotVolumeQuarter { get; set; }

        [JsonProperty("200025")]
        public int IsRise { get; set; }

        [JsonProperty("700006")]
        public decimal PB { get; set; }

        [JsonProperty("700005")]
        public long Cap { get; set; }

        [JsonProperty("200055")]
        public double BuySideCount { get; set; }

        [JsonProperty("200054")]
        public double SellSideCount { get; set; }

        [JsonProperty("200057")]
        public decimal SellSidePercent { get; set; }

        [JsonProperty("200056")]
        public decimal BuySidePercent { get; set; }
    }
}