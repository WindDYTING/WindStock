# 台股報價擷取

## 資料來源
[台灣證卷交易所 https://mis.twse.com.tw/stock](https://mis.twse.com.tw/stock/group.jsp?ex=tse&currPage=0&ind=TIDX&bp=0&type=fixed)
[鉅亨網](https://www.cnyes.com/twstock/)

## Getting Started
取得 TwStock 台股報價
```c#
var crawler = new TwStockCrawler();

var data = crawler.Get("2330", TradeType.TSE);
Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
```

訂閱 TwStock 台股報價
```c#
_exchange = new TwStockExchange(crawler);
_exchange.Subscribe("2330", TradeType.TSE, OnMessage);

private static void OnMessage(TwSourceStockData data)
{
    Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
}
```

取得鉅亨網台股報價
```c#
var crawler = new AnueStockCrawler();

var data = crawler.Get("2330", TradeType.TSE);
Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
```

訂閱 鉅亨網 台股報價
```c#
_exchange = new AnueStockExchange(crawler);
_exchange.Subscribe("2330", TradeType.TSE, OnMessage);

private static void OnMessage(AnueSourceStockData data)
{
    Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
}
