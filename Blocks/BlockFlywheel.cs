using HarpTech.BlockEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace HarpTech.Blocks
{
    class BlockFlywheel : BlockMPBase
    {
        BlockFacing powerOutFacing;
        BlockPos[] fakePos = new BlockPos[8];

        public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
        {
            BlockFacing[] orient = SuggestedHVOrientation(byPlayer, blockSel);
            orient[0] = orient[0].GetCW();

            BlockPos checkPos = blockSel.Position.Copy();

            if(!CheckSurrounding(checkPos, orient[0])) { return false; }

            if(base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode))
            {
                PlaceFakeBlocks(blockSel.Position);
                return true;
            }

            return false;
        }

        bool CheckSurrounding(BlockPos checkPos, BlockFacing dir)
        {
            checkPos.Add(dir);
            if (api.World.BlockAccessor.GetBlock(checkPos).Id != 0) { return false; }
            fakePos[0] = checkPos.Copy();

            checkPos.Add(BlockFacing.DOWN);
            if (api.World.BlockAccessor.GetBlock(checkPos).Id != 0) { return false; }
            fakePos[1] = checkPos.Copy();

            checkPos.Add(dir.Opposite);
            if (api.World.BlockAccessor.GetBlock(checkPos).Id != 0) { return false; }
            fakePos[2] = checkPos.Copy();

            checkPos.Add(dir.Opposite);
            if (api.World.BlockAccessor.GetBlock(checkPos).Id != 0) { return false; }
            fakePos[3] = checkPos.Copy();

            checkPos.Add(BlockFacing.UP);
            if (api.World.BlockAccessor.GetBlock(checkPos).Id != 0) { return false; }
            fakePos[4] = checkPos.Copy();

            checkPos.Add(BlockFacing.UP);
            if (api.World.BlockAccessor.GetBlock(checkPos).Id != 0) { return false; }
            fakePos[5] = checkPos.Copy();

            checkPos.Add(dir);
            if (api.World.BlockAccessor.GetBlock(checkPos).Id != 0) { return false; }
            fakePos[6] = checkPos.Copy();

            checkPos.Add(dir);
            if (api.World.BlockAccessor.GetBlock(checkPos).Id != 0) { return false; }
            fakePos[7] = checkPos.Copy();

            return true;
        }

        void PlaceFakeBlocks(BlockPos parentPos)
        {
            Block fakeBlock = api.World.GetBlock(new AssetLocation("harptech:fakeflywheelblock"));

            for (int i = 0; i < fakePos.Length; i++)
            {
                api.World.BlockAccessor.SetBlock(fakeBlock.Id, fakePos[i]);
                BEFakeBlock fakeBE = api.World.BlockAccessor.GetBlockEntity(fakePos[i]) as BEFakeBlock;
                fakeBE.parentPos = parentPos;
                fakeBE.MarkDirty();
            }
        }

        public override void OnBlockRemoved(IWorldAccessor world, BlockPos pos)
        {
            BlockFacing face = BlockFacing.FromCode(Variant["side"]);
            face = face.GetCW();
            BlockPos remPos = pos.Copy();

            remPos.Add(face);
            world.BlockAccessor.SetBlock(0, remPos);

            remPos.Add(BlockFacing.DOWN);
            world.BlockAccessor.SetBlock(0, remPos);

            remPos.Add(face.Opposite);
            world.BlockAccessor.SetBlock(0, remPos);

            remPos.Add(face.Opposite);
            world.BlockAccessor.SetBlock(0, remPos);

            remPos.Add(BlockFacing.UP);
            world.BlockAccessor.SetBlock(0, remPos);

            remPos.Add(BlockFacing.UP);
            world.BlockAccessor.SetBlock(0, remPos);

            remPos.Add(face);
            world.BlockAccessor.SetBlock(0, remPos);

            remPos.Add(face);
            world.BlockAccessor.SetBlock(0, remPos);

            base.OnBlockRemoved(world, pos);
        }

        public override void DidConnectAt(IWorldAccessor world, BlockPos pos, BlockFacing face)
        {
        }

        public override bool HasMechPowerConnectorAt(IWorldAccessor world, BlockPos pos, BlockFacing face)
        {
            return face == powerOutFacing;
        }

        public override void OnLoaded(ICoreAPI api)
        {
            powerOutFacing = BlockFacing.FromCode(Variant["side"]).Opposite;

            base.OnLoaded(api);
        }
    }
}
