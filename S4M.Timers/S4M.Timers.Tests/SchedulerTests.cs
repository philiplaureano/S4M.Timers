using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using S4M.Core;
using Xunit;

namespace S4M.Timers.Tests
{
    public class SchedulerTests
    {
        [Fact(DisplayName = "We should be able to schedule a message to be told once")]
        public async Task ShouldBeAbleToScheduleATellOnce()
        {
            var sender = A.Fake<IStateMachine>();
            var message = Guid.NewGuid();
            var receiver = A.Fake<IStateMachine>();

            A.CallTo(() => receiver.TellAsync(message, A<CancellationToken>.Ignored))
                .Returns(Task.CompletedTask);

            var delay = 1.Seconds();
            Context.System.Scheduler.ScheduleTellOnce(delay, receiver, message, sender);
            await Task.Delay(2.Seconds());

            A.CallTo(() => receiver.TellAsync(message, A<CancellationToken>.Ignored))
                .MustHaveHappened();
        }

        [Fact(DisplayName = "We should be able to schedule a message to be told repeatedly, until cancelled")]
        public async Task ShouldBeAbleToScheduleARepeatedTell()
        {
            var sender = A.Fake<IStateMachine>();
            var message = Guid.NewGuid();
            var receiver = A.Fake<IStateMachine>();

            A.CallTo(() => receiver.TellAsync(message, A<CancellationToken>.Ignored))
                .Returns(Task.CompletedTask);

            // Ensure that the state machine is called multiple times during the interval
            var interval = 50.Milliseconds();

            var deadline = 1.Seconds();
            var initialDelay = 0.Seconds();

            // Start the timer and estimate the actual deadline
            var startTime = DateTime.UtcNow;
            var cancelable = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(initialDelay, interval, receiver,
                message,
                sender);

            cancelable.CancelAfter(deadline);

            // Block until the actual deadline is over
            var actualDeadline = startTime + deadline;
            while (DateTime.UtcNow <= actualDeadline)
            {
                await Task.Delay(1.Milliseconds());
            }

            var numberOfExpectedCalls = Convert.ToInt32(deadline.TotalMilliseconds / interval.TotalMilliseconds);
            Assert.True(numberOfExpectedCalls > 15);

            A.CallTo(() => receiver.TellAsync(message, A<CancellationToken>.Ignored))
                .MustHaveHappened(numberOfExpectedCalls, Times.OrLess);
        }
    }
}