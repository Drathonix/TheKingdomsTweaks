using HarmonyLib;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Channels;
using TheKingdomsTweaks.Block;
using TheKingdomsTweaks.Network;
using TheKingdomsTweaks.Stances;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace TheKingdomsTweaks
{
    public class TheKingdomsTweaksModSystem : ModSystem
    {
        private const string HarmonyId = "thekingdomstweaks";
        private Harmony _harmony;

        public StancesModSystem stances;

        // Called on server and client
        // Useful for registering block/entity classes on both sides
        public override void Start(ICoreAPI api)
        {
            _harmony = new Harmony(HarmonyId);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            api.RegisterBlockClass("TKTClay", typeof(BlockClay));
            stances = api.ModLoader.GetModSystem<StancesModSystem>();
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            //base.StartServerSide(api);
            //api.ChatCommands.Create();
        }
        public override void StartClientSide(ICoreClientAPI api)
        {

        }

        public override void Dispose()
        {
            base.Dispose();

            _harmony?.UnpatchAll(HarmonyId);
        }
    }
}
