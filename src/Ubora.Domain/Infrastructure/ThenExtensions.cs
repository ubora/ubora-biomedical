using System;
using System.Diagnostics;

namespace Ubora.Domain.Infrastructure
{
    [DebuggerStepThrough]
    public static class ThenExtensions 
    {
        public static void Then<T>(this T @this, Action<T> action)
        {
            action(@this);
        }
        
        public static TResult Then<T, TResult>(this T @this, Func<T, TResult> func)
        {
            return func(@this);
        }
    }
}