using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace TheKingdomsTweaks.Block;

public class BlockClay: BlockSoilDeposit
{
    public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1)
    {
        // Basically copied from the vanilla ore code shut up.
        dropQuantityMultiplier *= byPlayer != null ? byPlayer.Entity.Stats.GetBlended("clayDropRate") : 1f;
        EnumHandling handling = EnumHandling.PassThrough;
        foreach (BlockBehavior blockBehavior in this.BlockBehaviors)
        {
            blockBehavior.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier, ref handling);
            if (handling == EnumHandling.PreventSubsequent)
                return;
        }
        if (handling == EnumHandling.PreventDefault)
            return;
        if (world.Side == EnumAppSide.Server && (byPlayer == null || byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative))
        {
            ItemStack[] drops = this.GetDrops(world, pos, byPlayer, dropQuantityMultiplier);
            if (drops != null)
            {
                for (int index = 0; index < drops.Length; ++index)
                    world.SpawnItemEntity(drops[index], pos);
            }
            world.PlaySoundAt(this.Sounds.GetBreakSound(byPlayer), pos, -0.5, byPlayer);
        }
        this.SpawnBlockBrokenParticles(pos);
        world.BlockAccessor.SetBlock(0, pos);
        if (byPlayer == null || byPlayer.WorldData.CurrentGameMode == EnumGameMode.Creative)
            return;
        CollectibleObject collectible = byPlayer?.InventoryManager.ActiveHotbarSlot.Itemstack?.Collectible;
    }
}
