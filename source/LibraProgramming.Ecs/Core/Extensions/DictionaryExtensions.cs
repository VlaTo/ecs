using System;
using System.Collections.Generic;

namespace LibraProgramming.Ecs.Core.Extensions
{
    internal static class DictionaryExtensions
    {
        public static bool Remove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, out TValue value)
        {
            if (null == dictionary)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (dictionary.TryGetValue(key, out value))
            {
                return dictionary.Remove(key);
            }

            value = default;

            return false;
        }
    }
}