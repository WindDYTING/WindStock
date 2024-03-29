using System.Threading.Tasks;
using WindStock.Models;

namespace WindStock
{
    public interface ICrawler<TData>
    {
        Task<TData> GetAsync(string commId, TradeType tradeType);
        Task<TData> GetAsync(string commId);
        TwSourceStockData Get(string commId);
        TwSourceStockData Get(string commId, TradeType tradeType);
    }
}