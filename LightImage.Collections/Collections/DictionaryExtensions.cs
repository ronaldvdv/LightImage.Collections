using System.Linq;

namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        // https://stackoverflow.com/questions/469202/best-way-to-remove-multiple-items-matching-a-predicate-from-a-c-sharp-dictionary
        public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dict, Func<TKey, TValue, bool> predicate)
        {
            var items = dict.Where(k => predicate(k.Key, k.Value)).ToList();
            foreach (var item in items)
                dict.Remove(item.Key);
        }
    }
}