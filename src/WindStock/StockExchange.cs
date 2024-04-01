using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;

namespace WindStock
{
    public abstract class StockExchange<T> : IExchange<T>
    {
        protected ICrawler<T> Crawler;
        private readonly ConcurrentDictionary<SubscribeKey, IObserver<T>> _allSubscribe = new ();
        private int _exchangeThreadFlag;
        private Thread _exchangeThread;

        protected StockExchange() { }

        protected StockExchange(ICrawler<T> crawler)
        {
            Preconditions.NotNull(crawler);

            Crawler = crawler;
        }

        public IDisposable Subscribe(string commId, TradeType type, Action<T> onMessage)
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

            return new StockUnsubscriber(_allSubscribe, key);
        }

        public void Unsubscribe(IDisposable disposable)
        {
            disposable.Dispose();
        }

        public void Dispose()
        {
            _allSubscribe.Clear();
            Crawler?.Dispose();
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

        private async void StartExchange()
        {
            while (true)
            {
                SpinWait.SpinUntil(() => !TwStockUtils.IsClose());

                foreach (var (key, o) in _allSubscribe)
                {
                    try
                    {
                        var data = await Crawler.GetAsync(key.CommId, key.TType);
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

        private class StockUnsubscriber : IDisposable
        {
            private readonly ConcurrentDictionary<SubscribeKey, IObserver<T>> _allSubscribe;
            private readonly SubscribeKey _key;

            public StockUnsubscriber(ConcurrentDictionary<SubscribeKey, IObserver<T>> allSubscribe, SubscribeKey key)
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

        public virtual IDisposable Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }
    }
}