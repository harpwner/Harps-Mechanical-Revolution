using HarpTech.BlockEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace HarpTech.Blocks
{
    class BlockBoiler : Block
    {
        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            if (byPlayer.InventoryManager.ActiveHotbarSlot.Empty) { return base.OnBlockInteractStart(world, byPlayer, blockSel); }

            ItemStack stack = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack;

            if (stack == null) { return base.OnBlockInteractStart(world, byPlayer, blockSel); }

            BEBoiler boilerEntity = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BEBoiler;

            if (boilerEntity != null)
            {
                return boilerEntity.BucketInteract(stack.Collectible as BlockLiquidContainerBase, byPlayer);
            }
            return base.OnBlockInteractStart(world, byPlayer, blockSel);
        }
    }
}
