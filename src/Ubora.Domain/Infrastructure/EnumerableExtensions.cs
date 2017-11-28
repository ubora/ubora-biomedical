using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace System.Linq
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (T item in list)
            {
                action(item);
            }
        }
    }
}
