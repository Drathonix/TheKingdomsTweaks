using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using TheKingdomsTweaks.API;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;


namespace TheKingdomsTweaks.Patches;

[HarmonyPatch(typeof(EntityPlayer), nameof(EntityAgent.OnGameTick)), HarmonyPriority(401)]
public class PatchEntityPlayer
{
    internal static void Postfix(EntityPlayer __instance, float dt)
    {
        if (__instance.Api.Side.IsServer())
        {
            float sunBrightnessBurnPoint = __instance.Stats.GetBlended("tkt:burnsInSunBrightness")-1;
            if (sunBrightnessBurnPoint > 0)
            {
                if(sunBrightnessBurnPoint <= __instance.World.BlockAccessor.GetLightLevel(__instance.Pos.AsBlockPos, EnumLightLevelType.Sunbrightness))
                {
                    float sunLightBurnPoint = __instance.Stats.GetBlended("tkt:burnsInSunLight")-1;
                    if (sunLightBurnPoint > 0)
                    {
                        if(sunLightBurnPoint <= __instance.World.BlockAccessor.GetLightLevel(__instance.Pos.AsBlockPos, EnumLightLevelType.OnlySunLight))
                        {
                            int timeInSun = __instance.WatchedAttributes.GetAsInt("tkttimeinsun");
                            timeInSun++;
                            if (timeInSun > 100 && !__instance.IsOnFire)
                            {
                                __instance.Ignite();
                            }
                            __instance.WatchedAttributes.SetInt("tkttimeinsun", timeInSun);
                        }
                        else
                        {
                            __instance.WatchedAttributes.SetInt("tkttimeinsun", 0);
                        }
                    }
                }
                else
                {
                    __instance.WatchedAttributes.SetInt("tkttimeinsun", 0);
                }
            }
        }

        if (__instance.Api.Side.IsClient())
        {
            var t = __instance.WatchedAttributes.GetAsInt("tkttimeinsun");
            if (t <= 100 && t > 0)
            {
                int num2 = Math.Min(Entity.FireParticleProps.Length - 1, __instance.Api.World.Rand.Next(Entity.FireParticleProps.Length + 1));
                AdvancedParticleProperties advancedParticleProperties = Entity.FireParticleProps[num2];
                advancedParticleProperties.basePos.Set(__instance.Pos.X, __instance.Pos.InternalY + (double)(__instance.SelectionBox.YSize / 2f), __instance.Pos.Z);
                advancedParticleProperties.PosOffset[0].var = __instance.SelectionBox.XSize / 2f;
                advancedParticleProperties.PosOffset[1].var = __instance.SelectionBox.YSize / 2f;
                advancedParticleProperties.PosOffset[2].var = __instance.SelectionBox.ZSize / 2f;
                advancedParticleProperties.Velocity[0].avg = (float)__instance.Pos.Motion.X * 10f;
                advancedParticleProperties.Velocity[1].avg = (float)__instance.Pos.Motion.Y * 5f;
                advancedParticleProperties.Velocity[2].avg = (float)__instance.Pos.Motion.Z * 10f;
                advancedParticleProperties.Quantity.avg = GameMath.Sqrt(advancedParticleProperties.PosOffset[0].var + advancedParticleProperties.PosOffset[1].var + advancedParticleProperties.PosOffset[2].var) * num2 switch
                {
                    1 => 3f,
                    0 => 0.5f,
                    _ => 1.25f,
                };
                __instance.Api.World.SpawnParticles(advancedParticleProperties);
            }
        }
    }
}
