
using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace HarpTech.Blocks
{
    class BlockGearbox : BlockMPBase
    {
        BlockFacing outFace;

        public override void OnLoaded(ICoreAPI api)
        {
            outFace = BlockFacing.FromCode(Variant["side"]);

            base.OnLoaded(api);
        }

        public override void DidConnectAt(IWorldAccessor world, BlockPos pos, BlockFacing face)
        {
        }

        public override bool HasMechPowerConnectorAt(IWorldAccessor world, BlockPos pos, BlockFacing face)
        {
            if (face == outFace || face == outFace.Opposite) return true;

            return false;
        }
    }
}
