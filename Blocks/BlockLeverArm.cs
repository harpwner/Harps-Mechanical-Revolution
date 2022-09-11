using HarpTech.BEBehaviors;
using HarpTech.BlockEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace HarpTech.Blocks
{
    class BlockLeverArm : Block
    {
        const int horizontalSize = 5;

        public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
        {
            BlockFacing[] orient = SuggestedHVOrientation(byPlayer, blockSel);
            orient[0] = orient[0].GetCW();

            BlockPos checkPos = blockSel.Position.Copy();
            checkPos.Add(orient[0]).Add(orient[0]);

            for(int i = 0; i < horizontalSize; i++)
            {
                if(api.World.BlockAccessor.GetBlock(checkPos).Id != 0)
                {
                    return false;
                }

                checkPos.Add(orient[0].Opposite);
            }

            PlaceFakeBlocks(orient[0], checkPos.AddCopy(orient[0]), blockSel.Position);

            return base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
        }

        void PlaceFakeBlocks(BlockFacing face, BlockPos start, BlockPos parentPos)
        {
            Block fakeBlock = api.World.GetBlock(new AssetLocation("harptech:fakeleverblock"));
            int skip = 2;

            for (int i = 0; i < horizontalSize; i++)
            {
                if(i != skip) 
                { 
                    api.World.BlockAccessor.SetBlock(fakeBlock.Id, start);
                    BEFakeBlock fakeBE = api.World.BlockAccessor.GetBlockEntity(start) as BEFakeBlock;
                    fakeBE.parentPos = parentPos;
                    fakeBE.MarkDirty();
                }
                start.Add(face);
            }
        }

        public override void OnBlockRemoved(IWorldAccessor world, BlockPos pos)
        {
            BlockFacing face = BlockFacing.FromCode(Variant["side"]);
            face = face.GetCW();
            BlockPos remPos = pos.Copy();
            remPos.Add(face).Add(face);

            int skip = 2;
            for (int i = 0; i < horizontalSize; i++)
            {
                if (i != skip) { api.World.BlockAccessor.SetBlock(0, remPos); }
                remPos.Add(face.Opposite);
            }

            base.OnBlockRemoved(world, pos);
        }
    }
}
