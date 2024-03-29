using System;

namespace WindStock
{
    public static class Observer
    {
        public static IObserver<T> Create<T>(Action<T> onNext)
        {
            Preconditions.NotNull(onNext);

            return new DelegateObserver<T>(onNext);
        }

        public static IObserver<T> Create<T>(Action<T> onNext, Action<Exception> onError)
        {
            Preconditions.NotNull(onNext);
            Preconditions.NotNull(onError);

            return new DelegateObserver<T>(onNext, onError);
        }

        public static IObserver<T> Create<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            Preconditions.NotNull(onNext);
            Preconditions.NotNull(onError);
            Preconditions.NotNull(onCompleted);

            return new DelegateObserver<T>(onNext, onError, onCompleted);
        }
    }
}
