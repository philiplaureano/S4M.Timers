using System;
using System.Threading;

namespace S4M.Timers
{
    // Note: This interface was copied from Akka.NET to make easier for S4M users to use the same timers with Akka.NET
    public interface ICancelable
    {
        void Cancel();
        
        bool IsCancellationRequested { get; }
        
        CancellationToken Token { get; }
        
        void CancelAfter(TimeSpan delay);
        
        void CancelAfter(int millisecondsDelay);
        
        void Cancel(bool throwOnFirstException);
    }
}