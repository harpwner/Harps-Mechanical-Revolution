using HarpTech.BEBehaviors;
using Vintagestory.GameContent.Mechanics;
using Vintagestory.GameContent;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace HarpTech.Blocks
{
    class BlockAeolipile : BlockMPBase
    {
        BlockFacing powerOutFacing;

        public override void OnLoaded(ICoreAPI api)
        {
            powerOutFacing = BlockFacing.FromCode(Variant["side"]).Opposite;

            base.OnLoaded(api);
        }

        public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
        {
            BlockPos pos = new BlockPos(blockSel.Position.X, blockSel.Position.Y - 1, blockSel.Position.Z);
            Block blockBelow = world.BlockAccessor.GetBlock(pos);

            if (blockBelow is BlockFirepit || blockBelow.Id == 0)
            {
                return base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
            }

            return false;
        }

        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            if (byPlayer.InventoryManager.ActiveHotbarSlot.Empty) { return base.OnBlockInteractStart(world, byPlayer, blockSel); }

            ItemStack stack = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack;

            if (stack == null) { return base.OnBlockInteractStart(world, byPlayer, blockSel); }

            BEBehaviorAeolipile behavior = world.BlockAccessor.GetBlockEntity(blockSel.Position)?.GetBehavior<BEBehaviorAeolipile>();

            if (behavior != null)
            {
                return behavior.BucketInteract(stack.Collectible as BlockLiquidContainerBase, byPlayer);
            }
            return base.OnBlockInteractStart(world, byPlayer, blockSel);
        }

        public override void DidConnectAt(IWorldAccessor world, BlockPos pos, BlockFacing face)
        {
            //empty on purpose
        }

        public override bool HasMechPowerConnectorAt(IWorldAccessor world, BlockPos pos, BlockFacing face)
        {
            return face == powerOutFacing;
        }
    }
}
