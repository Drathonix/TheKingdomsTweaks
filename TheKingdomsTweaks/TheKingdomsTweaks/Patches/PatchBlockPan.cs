using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace TheKingdomsTweaks.Patches;


[HarmonyPatch(typeof(BlockPan), nameof(BlockPan.OnHeldInteractStop)), HarmonyPriority(401)]
public class PatchBlockPan
{
    private static MethodInfo targetMethod = AccessTools.Method(typeof(BlockPan),"CreateDrop");

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var replMethod = typeof(PatchBlockPan).GetMethod("SpecCreateDrop");
        return Transpilers.MethodReplacer(instructions,targetMethod,replMethod);

    }

    public static void SpecCreateDrop(BlockPan pan, EntityAgent byEntity, string fromBlockCode)
    {
        targetMethod.Invoke(pan,[byEntity,fromBlockCode]);
        float n = Math.Max(byEntity.Stats.GetBlended("tkt:panningDropRate") -1,0);
        while(n > 1)
        {
            n--;
            targetMethod.Invoke(pan, [byEntity, fromBlockCode]);
        }
        if (byEntity.Api.World.Rand.NextDouble() < n)
        {
            targetMethod.Invoke(pan, [byEntity, fromBlockCode]);
        }
    }
}