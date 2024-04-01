using WindStock.Models;

namespace WindStock
{
    public class TwStockExchange : StockExchange<TwSourceStockData>
    {
        public TwStockExchange()
        {
            Crawler = new TwStockCrawler();
        }
        
        public TwStockExchange(TwStockCrawler twStockCrawler) : base(twStockCrawler)
        {
        }
    }
}
