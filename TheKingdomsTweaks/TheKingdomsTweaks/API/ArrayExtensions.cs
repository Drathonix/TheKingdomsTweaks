using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace TheKingdomsTweaks.API;

public static class ArrayExtensions
{

    extension<V>(V[] arr)
    {
        public V GetOrDefault(int idx, V defaultValue = default)
        {
            if (idx < 0 || idx >= arr.Length)
            {
                return defaultValue;
            }
            else
            {
                return arr[idx];
            }   
        }
    }
}
