using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace HarpTech.BlockEntities
{
    class BEPistonPump : BEFluidProducerBase
    {
        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);
        }

        public override List<BlockPos> GetConnections()
        {
            List<BlockPos> connections = new List<BlockPos>();



            return connections;
        }
    }
}
