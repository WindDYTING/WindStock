using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;
using WindStock.Models;

namespace WindStock
{
    public class TwStockExchange : IExchange<TwSourceStockData>, IDisposable
    {
        private readonly ICrawler<TwSourceStockData> _crawler;

        private readonly ConcurrentDictionary<SubscribeKey, IObserver<TwSourceStockData>> _allSubscribe = new ();

        private int _exchangeThreadFlag;
        private Thread _exchangeThread;

        public TwStockExchange(ICrawler<TwSourceStockData> crawler)
        {
            _crawler = crawler;
        }

        public TwStockExchange()
        {
            _crawler = new TwStockCrawler();
        }

        public IDisposable Subscribe(string commId, TradeType type, Action<TwSourceStockData> onMessage)
        {
            Preconditions.NotNullOrEmpty(commId);
            Preconditions.NotNull(onMessage);
            Preconditions.NotNull(type);

            var key = new SubscribeKey(commId, type);
            if (_allSubscribe.ContainsKey(key))
            {
                return Disposable.Empty;
            }

            EnsureStartExchange();

            var o = Observer.Create(onMessage);
            _allSubscribe.TryAdd(key, o);

            return new TwStockUnsubscriber(_allSubscribe, key);
        }

        public IDisposable Subscribe(IObserver<TwSourceStockData> observer)
        {
            throw new NotSupportedException();
        }

        public void Unsubscribe(IDisposable disposable)
        {
            disposable.Dispose();
        }

        public void Dispose()
        {
            _allSubscribe.Clear();
        }

        private void EnsureStartExchange()
        {
            if (Interlocked.CompareExchange(ref _exchangeThreadFlag, 1, 0) == 0)
            {
                _exchangeThread = new Thread(StartExchange)
                {
                    IsBackground = true
                };
                _exchangeThread.Start();
            }
        }

        private void StartExchange()
        {
            while (true)
            {
                SpinWait.SpinUntil(() => !TwStockUtils.IsClose());

                foreach (var (key, o) in _allSubscribe)
                {
                    try
                    {
                        var data = _crawler.Get(key.CommId, key.TType);
                        o.OnNext(data);
                    }
                    catch (HttpRequestException)
                    {

                    }
                    catch (Exception e)
                    {
                        o.OnError(e);
                    }
                }

                SpinWait.SpinUntil(() => false, 500);
            }
        }

        private record SubscribeKey(string CommId, TradeType TType)
        {
            public override string ToString()
            {
                return $"{CommId}_{TType}";
            }
        }

        private class TwStockUnsubscriber : IDisposable
        {
            private readonly ConcurrentDictionary<SubscribeKey, IObserver<TwSourceStockData>> _allSubscribe;
            private readonly SubscribeKey _key;

            public TwStockUnsubscriber(ConcurrentDictionary<SubscribeKey, IObserver<TwSourceStockData>> allSubscribe, SubscribeKey key)
            {
                _allSubscribe = allSubscribe;
                _key = key;
            }

            public void Dispose()
            {
                if (_allSubscribe.ContainsKey(_key))
                {
                    _allSubscribe.TryRemove(_key, out _);
                }
            }
        }

    }

    public interface IExchange<TData> : IObservable<TData>
    {
        IDisposable Subscribe(string commId, TradeType type, Action<TData> onMessage);

        void Unsubscribe(IDisposable disposable);
    }
}
