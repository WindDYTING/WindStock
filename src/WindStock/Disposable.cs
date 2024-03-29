using System;

namespace WindStock
{
    public class Disposable : IDisposable
    {
        public static IDisposable Empty = new Disposable();

        public void Dispose()
        {
            // Do nothing...
        }
    }
}
