using HarpTech.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace HarpTech.BlockEntities
{
    class BEFakeBlock : BlockEntity
    {
        public BlockPos parentPos;

        public override void OnBlockBroken(IPlayer byPlayer = null)
        {
            Api.World.BlockAccessor.BreakBlock(parentPos, byPlayer);

            base.OnBlockBroken(byPlayer);
        }

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            tree.SetBlockPos("parent", this.parentPos);

            base.ToTreeAttributes(tree);
        }

        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
        {
            this.parentPos = tree.GetBlockPos("parent");

            base.FromTreeAttributes(tree, worldAccessForResolve);
        }
    }
}
