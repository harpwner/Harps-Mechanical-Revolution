using HarpTech.Enums;
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
    class BEFluidConsumerBase : BEFluidNetworkBase
    {
        public virtual float Consumption => 10;
        public float efficiency = 0;

        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);
        }

        public override List<BlockPos> GetConnections()
        {
            return base.GetConnections();
        }

        public virtual float ConsumptionTick(float dt, float capacity)
        {
            float consumption = Consumption * dt;

            if (capacity <= consumption)
            {
                this.efficiency = capacity / consumption;
                return capacity;
            }

            this.efficiency = 1;
            return consumption;
        }
    }
}
