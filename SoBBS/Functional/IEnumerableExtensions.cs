using System;
using System.Collections.Generic;

namespace Sobbs.Functional
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach(var element in collection)
                action(element);
        }
    }
}

