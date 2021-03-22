using System;
using S4M.Core;

namespace S4M.Timers
{
    public static class Context
    {
        public static class System
        {
            public static class Scheduler
            {
                public static void ScheduleTellOnce(TimeSpan delay, ICanTellAsync receiver, object message,
                    /* ignored */ICanTellAsync sender)
                {
                    Tell.Once(delay, receiver, message);
                }

                public static ICancelable ScheduleTellOnceCancelable(TimeSpan delay, ICanTellAsync receiver,
                    object message,
                    /* ignored */ICanTellAsync sender)
                {
                    return Tell.Once(delay, receiver, message);
                }

                public static void ScheduleTellRepeatedly(TimeSpan initialDelay, TimeSpan interval,
                    ICanTellAsync receiver,
                    object message, /* ignored */ ICanTellAsync sender)
                {
                    Tell.Repeatedly(initialDelay, interval, receiver, message);
                }

                public static ICancelable ScheduleTellRepeatedlyCancelable(TimeSpan initialDelay, TimeSpan interval,
                    ICanTellAsync receiver,
                    object message, /* ignored */ ICanTellAsync sender)
                {
                    return Tell.Repeatedly(initialDelay, interval, receiver, message);
                }
            }
        }
    }
}