using System.Collections.Generic;

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static bool AllEqual<T>(this IEnumerable<T> items, IEqualityComparer<T> comparer)
        {
            var any = false;
            var first = default(T);
            foreach (var item in items)
            {
                if (!any)
                {
                    any = true;
                    first = item;
                }
                else if (!comparer.Equals(item, first))
                    return false;
            }
            return true;
        }

        public static bool AllEqual<T>(this IEnumerable<T> items)
        {
            return AllEqual(items, EqualityComparer<T>.Default);
        }

        public static TItem ArgMax<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> value, IComparer<TValue> comparer)
        {
            return ArgMinMax(items, value, comparer, true);
        }

        public static TItem ArgMax<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> value)
        {
            return ArgMax(items, value, Comparer<TValue>.Default);
        }

        public static TItem ArgMin<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> value)
        {
            return ArgMin(items, value, Comparer<TValue>.Default);
        }

        public static TItem ArgMin<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> value, IComparer<TValue> comparer)
        {
            return ArgMinMax(items, value, comparer, false);
        }

        public static int FindIndex<T>(this IReadOnlyList<T> list, Predicate<T> predicate)
        {
            for (var i = 0; i < list.Count; i++)
                if (predicate(list[i]))
                    return i;
            return -1;
        }

        private static TItem ArgMinMax<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> value, IComparer<TValue> comparer, bool maximize)
        {
            var any = false;
            var bestValue = default(TValue);
            var bestItem = default(TItem);
            foreach (var item in items)
            {
                var itemValue = value(item);
                if (!any || (comparer.Compare(itemValue, bestValue) > 0 == maximize))
                {
                    bestValue = itemValue;
                    bestItem = item;
                }
                any = true;
            }
            return bestItem;
        }
    }
}