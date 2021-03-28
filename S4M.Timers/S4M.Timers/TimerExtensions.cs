using System;

namespace S4M.Timers
{
    public static class TimerExtensions
    {
        public static IDisposable RepeatEvery(this Action action, TimeSpan interval)
        {
            return action.RepeatEvery(TimeSpan.Zero, interval);
        }

        public static IDisposable RepeatEvery(this Action action, TimeSpan initialDelay, TimeSpan interval)
        {
            return new DisposableTimer(initialDelay, interval, action);
        }
    }
}