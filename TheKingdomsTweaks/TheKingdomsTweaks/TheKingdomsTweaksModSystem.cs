using HarmonyLib;
using System.Reflection;
using TheKingdomsTweaks.Block;
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

        // Called on server and client
        // Useful for registering block/entity classes on both sides
        public override void Start(ICoreAPI api)
        {
            _harmony = new Harmony(HarmonyId);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            api.RegisterBlockClass("TKTClay",typeof(BlockClay));
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            
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
