using System;

namespace WindStock
{
    public interface IExchange<TData> : IObservable<TData>, IDisposable
    {
        IDisposable Subscribe(string commId, TradeType type, Action<TData> onMessage);

        void Unsubscribe(IDisposable disposable);
    }
}