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

            // A trait or attribute that checks for day or night, presence of a roof, and light level, --- ashtoash 
            // and if day, unsheltered, and bright, sets the player on fire until conditions change back.
            try
            {
                int level = __instance.World.BlockAccessor.GetLightLevel(__instance.Pos.AsBlockPos, EnumLightLevelType.Sunbrightness);
                //var weather = __instance.Api.ModLoader.GetModSystem<WeatherSystemServer>();
                //weather.get
                if(level > 19)
                {
                    __instance.Ignite();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
