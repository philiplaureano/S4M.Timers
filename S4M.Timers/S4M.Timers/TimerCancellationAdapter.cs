using System;
using System.Threading;
using System.Threading.Tasks;

namespace S4M.Timers
{
    internal class TimerCancellationAdapter : ICancelable
    {
        private CancellationTokenSource _cts;
        private readonly IDisposable _disposableTimer;
        private Task _timerDisposalTask;
        private bool _disposed;

        internal TimerCancellationAdapter(CancellationTokenSource cts, IDisposable disposableTimer)
        {
            _cts = cts;
            _disposableTimer = disposableTimer;
            _timerDisposalTask = WaitUntilCancellation(cts, disposableTimer);
        }

        public void Cancel()
        {
            _cts.Cancel();
        }

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

        public bool IsCancellationRequested => _cts.IsCancellationRequested;

        public CancellationToken Token => _cts.Token;

        private async Task WaitUntilCancellation(CancellationTokenSource cts, IDisposable disposableTimer)
        {
            // Only dispose of the timer once
            if (_disposed)
                return;

            while (!cts.IsCancellationRequested)
            {
                await Task.Delay(100.Milliseconds());
            }

            _disposableTimer?.Dispose();
        }
    }
}