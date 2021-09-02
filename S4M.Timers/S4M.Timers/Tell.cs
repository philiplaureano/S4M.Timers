using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using S4M.Core;

namespace S4M.Timers
{
    public static class Tell
    {
        private static readonly ConcurrentDictionary<Guid, IDisposable> PendingTimers = new ConcurrentDictionary<Guid, IDisposable>();

        public static ICancelable Once(TimeSpan delay, ICanTellAsync receiver, object message)
        {
            return Once(delay, receiver, () => message);
        }

        public static ICancelable Once(TimeSpan delay, ICanTellAsync receiver, Func<object> getMessage)
        {
            var cts = new CancellationTokenSource();
            var taskId = Guid.NewGuid();

            void OnCallback()
            {
                // Run the task only once
                var message = getMessage();
                Task.WaitAny(receiver.TellAsync(message, cts.Token));

                // Clean up the task itself
                if (PendingTimers.ContainsKey(taskId))
                {
                    PendingTimers[taskId]?.Dispose();
                    PendingTimers.TryRemove(taskId, out _);    
                }
            }

            Action timerTask = OnCallback;
            var pendingTimer = timerTask.RepeatEvery(delay, TimeSpan.Zero);

            PendingTimers[taskId] = pendingTimer;
            return new TimerCancellationAdapter(cts, pendingTimer);
        }

        public static ICancelable Repeatedly(TimeSpan initialDelay, TimeSpan interval, ICanTellAsync receiver,
            object message)
        {
            return Repeatedly(initialDelay, interval, receiver, () => message);
        }

        public static ICancelable Repeatedly(TimeSpan initialDelay, TimeSpan interval, ICanTellAsync receiver,
            Func<object> getMessage)
        {
            var cts = new CancellationTokenSource();
            var taskId = Guid.NewGuid();

            void OnCallback()
            {
                // Clean up the task after it has been cancelled
                if (PendingTimers.ContainsKey(taskId) && cts.IsCancellationRequested)
                {
                    PendingTimers[taskId]?.Dispose();
                    PendingTimers.TryRemove(taskId, out _);
                    return;
                }
                
                var message = getMessage();
                Task.WaitAny(receiver.TellAsync(message, cts.Token));
            }

            Action timerTask = OnCallback;
            var pendingTimer = timerTask.RepeatEvery(initialDelay, interval);

            PendingTimers[taskId] = pendingTimer;
            return new TimerCancellationAdapter(cts, pendingTimer);
        }
    }
}