using System.Threading.Tasks;

namespace WindStock
{
    public interface ICrawler<TData>
    {
        Task<TData> GetAsync(string commId, TradeType tradeType);
        Task<TData> GetAsync(string commId);
    }
}