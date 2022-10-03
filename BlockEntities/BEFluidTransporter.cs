using HarpTech.Enums;
using HarpTech.Interfaces;
using HarpTech.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace HarpTech.BlockEntities
{
    class BEFluidTransporter : BEFluidNetworkBase
    {
        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);
        }

        public override List<BlockPos> GetConnections()
        {
            List<BlockPos> connections = new List<BlockPos>();

            connections.Add(Pos.NorthCopy());
            connections.Add(Pos.SouthCopy());
            connections.Add(Pos.EastCopy());
            connections.Add(Pos.WestCopy());
            connections.Add(Pos.UpCopy());
            connections.Add(Pos.DownCopy());

            return connections;
        }
    }
}
