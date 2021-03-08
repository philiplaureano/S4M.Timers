using System;
using System.Threading;

namespace S4M.Timers
{
    internal class CancellationAdapter : ICancelable
    {
        private readonly CancellationTokenSource _cts;

        public CancellationAdapter(CancellationTokenSource cts)
        {
            _cts = cts ?? throw new ArgumentNullException(nameof(cts));
        }

        public void Cancel()
        {
            _cts.Cancel();
        }

        public bool IsCancellationRequested => _cts.IsCancellationRequested;
        public CancellationToken Token => _cts.Token;

        public void CancelAfter(TimeSpan delay)
        {
            _cts.CancelAfter(delay);
        }

        public void CancelAfter(int millisecondsDelay)
        {
            _cts.CancelAfter(millisecondsDelay);
        }

        public void Cancel(bool throwOnFirstException)
        {
            _cts.Cancel(throwOnFirstException);
        }
    }
}