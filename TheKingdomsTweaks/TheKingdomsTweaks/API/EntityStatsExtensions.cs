using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace TheKingdomsTweaks.API;
public static class EntityStatsExtensions
{

    extension(EntityStats? stats)
    {
        public bool Enabled(string key)
        {
            return (stats?.GetBlended(key) ?? 1) > 1;
        }
        public float GetZeroableFloat(string category)
        {
            return (stats?.GetBlended(category) ?? 1) - 1F;
        }
        public void OnDefaultedStat(string category, float defaultValue, Action<float> action)
        {
            float f = stats.GetOrDefault(category, defaultValue);
            if (f != -1)
            {
                action.Invoke(f);
            }
        }

        public float GetOrDefault(string category, float defaultValue)
        {
            // 0 if not present, -1 if disabled, >1 if present.
            var f = stats.GetZeroableFloat(category);
            if (f == 0)
            {
                return defaultValue;
            }
            return f;

        }
    }
}