using WindStock.Models;

namespace WindStock
{
    public class AnueStockExchange : StockExchange<AnueSourceStockData>
    {
        public AnueStockExchange(ICrawler<AnueSourceStockData> crawler) : base(crawler)
        {
        }

        public AnueStockExchange()
        {
            Crawler = new AnueStockCrawler();
        }
    }
}
