using System;
using System.Collections.Generic;

namespace BusRoutesFinder.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> TakeWhileInclusive<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (predicate is null)
            {
                yield break;
            }
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
                else
                {
                    yield return item;
                    yield break;
                }
            }
        } 
    }
}
