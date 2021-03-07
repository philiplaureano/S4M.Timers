using System;

namespace S4M.Timers
{
    public static class TimeSpanExtensions
    {
        public static TimeSpan Ticks(this int value)
        {
            return TimeSpan.FromTicks(value);
        }

        public static TimeSpan Nanoseconds(this int value)
        {
            return value.Milliseconds() / 1000000;
        }
        public static TimeSpan Milliseconds(this int value)
        {
            return TimeSpan.FromMilliseconds(value);
        }

        public static TimeSpan Seconds(this int value)
        {
            return TimeSpan.FromSeconds(value);
        }

        public static TimeSpan Minutes(this int value)
        {
            return TimeSpan.FromMinutes(value);
        }

        public static TimeSpan Hours(this int value)
        {
            return TimeSpan.FromHours(value);
        }

        public static TimeSpan Days(this int value)
        {
            return TimeSpan.FromDays(value);
        }

        public static TimeSpan Weeks(this int value)
        {
            return value.Days() * 7;
        }

        public static TimeSpan Months(this int value)
        {
            return value.Weeks() * 4;
        }

        public static TimeSpan Quarters(this int value)
        {
            return value.Months() * 3;
        }

        public static TimeSpan Years(this int value)
        {
            return value.Days() * 365;
        }
    }
}