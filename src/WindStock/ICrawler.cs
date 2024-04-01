using System;
using System.Threading.Tasks;
using WindStock.Models;

namespace WindStock
{
    public interface ICrawler<TData> : IDisposable
    {
        Task<TData> GetAsync(string commId, TradeType tradeType);
        Task<TData> GetAsync(string commId);
        TData Get(string commId);
        TData Get(string commId, TradeType tradeType);
    }
}