using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using TheKingdomsTweaks.Stances;
using Vintagestory.API.Common;

namespace TheKingdomsTweaks.Patches.Stances;

internal class PatchEntityPlayer
{
    // Stole some of this from playermodellib :P
    [HarmonyPatch(typeof(EntityPlayer), "updateEyeHeight")]
    public static void ApplyEyeHeightModifiers(EntityPlayer player, ref double newEyeheight, ref double newModelHeight)
    {
        float modifier = GetEyeHeightModifier(player);
        newEyeheight *= modifier;
        newModelHeight *= modifier;
    }

    public static float GetEyeHeightModifier(EntityPlayer player)
    {
        StancesModSystem stances = player.Api.ModLoader.GetModSystem<StancesModSystem>();
        Stance stance = stances.GetActiveStance(player);
        // I don't need to return here but it uses less math so why not.
        if (stance == stances.DefaultStance) return 1f;

        bool moving = (player.Controls.TriesToMove && player.Pos.Motion.LengthSq() > 0.00001) && !player.Controls.NoClip && !player.Controls.DetachedMode;
        bool walking = moving && player.OnGround;

        if (walking && !player.Controls.Backward && !player.Controls.Sneak && !player.Controls.IsClimbing && !player.Controls.IsFlying)
        {
            if (player.Controls.Sprint)
            {
                return stance.EyeHeightMultipliers.SprintMultiplier;
            }
            else
            {
                return stance.EyeHeightMultipliers.WalkMultiplier;
            }
        }

        return 1;
    }

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> codes = [.. instructions];
        MethodInfo callMethod = AccessTools.Method(typeof(PatchEntityPlayer), nameof(ApplyEyeHeightModifiers));
        var code = new List<CodeInstruction>(instructions);

        for (int i = 0; i < code.Count; i++)
        {
            var inst = code[i];
            if (inst.opcode == OpCodes.Stloc_S && inst.OperandIs("y"))
            {
                codes.InsertRange(i+1,
                [
                    new CodeInstruction(OpCodes.Ldarg_0),        // this (EntityPlayer)
                    new CodeInstruction(OpCodes.Ldloca_S, 4),    // ref newEyeheight
                    new CodeInstruction(OpCodes.Ldloca_S, 5),    // ref newModelHeight
                    new CodeInstruction(OpCodes.Call, callMethod)
                ]);
                break;
            }
        }
        return codes;
    }
    
}
