using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> AddToEnumerable<T>(this IEnumerable<T> source, T item)
        {
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
                yield return enumerator.Current;
            
            yield return item;
        }
    }
}
