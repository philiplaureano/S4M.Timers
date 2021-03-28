using System;
using System.Threading;

namespace S4M.Timers
{
    internal class DisposableTimer : IDisposable
    {
        private readonly Action _onTimerTick;
        private readonly Timer _timer;
        private bool _disposed;

        internal DisposableTimer(TimeSpan initialDelay, TimeSpan interval, Action onTimerTick)
        {
            _onTimerTick = onTimerTick;
            _timer = new Timer(OnTick, null, initialDelay, interval);
        }

        private void OnTick(object _)
        {
            _onTimerTick?.Invoke();
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _timer?.Dispose();

            _disposed = true;
        }
    }
}