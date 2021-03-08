using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using S4M.Core;

namespace S4M.Timers
{
    public static class Tell
    {
        private static readonly ConcurrentDictionary<Guid, Task> PendingTasks = new();

        public static ICancelable Once(TimeSpan delay, IStateMachine receiver, object message)
        {
            return Once(delay, receiver, () => message);
        }

        public static ICancelable Once(TimeSpan delay, IStateMachine receiver, Func<object> getMessage)
        {
            var cts = new CancellationAdapter(new CancellationTokenSource());
            var taskId = Guid.NewGuid();
            var timerTask = Task.Run(async () =>
            {
                // Pause and call the state machine
                await Task.Delay(delay, cts.Token);

                var message = getMessage();
                await receiver.TellAsync(message, cts.Token);

                // Clean up the task itself
                PendingTasks.TryRemove(taskId, out _);
            });

            PendingTasks[taskId] = timerTask;
            return cts;
        }

        public static ICancelable Repeatedly(TimeSpan initialDelay, TimeSpan interval, IStateMachine receiver,
            object message)
        {
            return Repeatedly(initialDelay, interval, receiver, () => message);
        }

        public static ICancelable Repeatedly(TimeSpan initialDelay, TimeSpan interval, IStateMachine receiver,
            Func<object> getMessage)
        {
            var cts = new CancellationAdapter(new CancellationTokenSource());
            var taskId = Guid.NewGuid();

            var timerTask = Task.Run(async () =>
            {
                // Pause using the initial delay 
                await Task.Delay(initialDelay, cts.Token);
                while (!cts.IsCancellationRequested)
                {
                    // Call the state machine itself and pause using the interval
                    var message = getMessage();
                    await receiver.TellAsync(message, cts.Token);
                    await Task.Delay(interval, cts.Token);
                }

                // Clean up the task itself
                PendingTasks.TryRemove(taskId, out _);
            });

            PendingTasks[taskId] = timerTask;
            return cts;
        }
    }
}