using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

using Vintagestory.API.Common;

namespace TheKingdomsTweaks.Patches;

[HarmonyPatch(typeof(AnimationMetaData), nameof(AnimationMetaData.Init)), HarmonyPriority(401)]

public class PatchAnimationMetaData_TriggerInjection
{
    internal static bool Prefix(AnimationMetaData __instance)
    {
        if (__instance.Code.Equals("ww_flap"))
        {
            //I WILL ERROR
            //__instance.TriggeredBy = new .Flap(__instance.TriggeredBy);
        }

        return true;
    }
}
