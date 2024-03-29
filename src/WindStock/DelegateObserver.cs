using System;

namespace WindStock
{
    public class DelegateObserver<T> : IObserver<T>
    {
        private readonly Action _onCompleted;
        private readonly Action<T> _onNext;
        private readonly Action<Exception> _onError;

        public DelegateObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            _onNext = onNext;
            _onCompleted = onCompleted;
            _onError = onError;
        }

        public DelegateObserver(Action<T> onNext)
        {
            _onNext = onNext;
        }

        public DelegateObserver(Action<T> onNext, Action<Exception> onError)
        {
            _onNext = onNext;
            _onError = onError;
        }

        public void OnCompleted()
        {
            _onCompleted?.Invoke();
        }

        public void OnError(Exception error)
        {
            _onError?.Invoke(error);
        }

        public void OnNext(T value)
        {
            _onNext?.Invoke(value);
        }
    }
}
