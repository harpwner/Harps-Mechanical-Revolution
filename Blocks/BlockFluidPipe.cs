using HarpTech.BlockEntities;
using HarpTech.Interfaces;
using System;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace HarpTech.Blocks
{
    class BlockFluidPipe : Block
    {
        public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
        {
            string neighbors = GetValidNeighbors(blockSel.Position);

            Block pipe = world.BlockAccessor.GetBlock(CodeWithVariant("attach", neighbors));

            if (pipe == null) { pipe = this; }

            if (pipe.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
            {
                world.BlockAccessor.SetBlock(pipe.BlockId, blockSel.Position, BlockLayersAccess.Solid);
                return true;
            }

            return false;
        }

        public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
        {
            UpdateModel(world, pos);

            base.OnNeighbourBlockChange(world, pos, neibpos);
        }

        public void UpdateModel(IWorldAccessor world, BlockPos pos)
        {
            string neighbors = GetValidNeighbors(pos);
            AssetLocation newPipe = CodeWithVariant("attach", neighbors);

            if (!Code.Equals(newPipe))
            {
                Block pipe = world.BlockAccessor.GetBlock(newPipe);
                if (pipe == null) { return; }
                world.BlockAccessor.ExchangeBlock(pipe.Id, pos);
                world.BlockAccessor.TriggerNeighbourBlockUpdate(pos);
                return;
            }
        }

        private bool CheckNeighbor(BlockEntity block, BlockFacing face)
        {
            if(block is BEFluidTransporter) { return true; }
            if(block is BEFluidProducerBase)
            {
                BEFluidProducerBase input = block as BEFluidProducerBase;
                //if(input.GetFacing().Equals(face)) { return true; }
                return true; //placeholder
            }
            if(block is BEFluidConsumerBase)
            {
                return true;
            }
            return false;
        }

        private String GetValidNeighbors(BlockPos pos)
        {
            StringBuilder sb = new StringBuilder();
            BlockPos checkPos = pos.Copy();
            IBlockAccessor accessor = api.World.BlockAccessor;
            BlockEntity curBlock;

            curBlock = accessor.GetBlockEntity(checkPos.NorthCopy());
            if(CheckNeighbor(curBlock, BlockFacing.SOUTH)) { sb.Append("n"); }
            curBlock = accessor.GetBlockEntity(checkPos.SouthCopy());
            if (CheckNeighbor(curBlock, BlockFacing.NORTH)) { sb.Append("s"); }
            curBlock = accessor.GetBlockEntity(checkPos.EastCopy());
            if (CheckNeighbor(curBlock, BlockFacing.WEST)) { sb.Append("e"); }
            curBlock = accessor.GetBlockEntity(checkPos.WestCopy());
            if (CheckNeighbor(curBlock, BlockFacing.EAST)) { sb.Append("w"); }
            curBlock = accessor.GetBlockEntity(checkPos.UpCopy());
            if (CheckNeighbor(curBlock, BlockFacing.DOWN)) { sb.Append("u"); }
            curBlock = accessor.GetBlockEntity(checkPos.DownCopy());
            if (CheckNeighbor(curBlock, BlockFacing.UP)) { sb.Append("d"); }

            if(sb.Length == 0) { sb.Append("none"); }

            return sb.ToString();
        }
    }
}
