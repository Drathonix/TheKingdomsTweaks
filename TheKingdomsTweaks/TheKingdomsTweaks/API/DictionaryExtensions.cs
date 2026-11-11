using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace TheKingdomsTweaks.API;

public static class DictionaryExtensions
{
    extension<K, V>(Dictionary<K,V> dict)
    {
        public V? GetNullable(K key)
        {
            if (dict.TryGetValue(key, out V? value))
            {
                return value;
            }
            else
            {
                return default;
            }   
        }
    }
}
