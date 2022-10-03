using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;

namespace HarpTech.Mechanics
{
    class FluidTransportMod : ModSystem
    {
        List<FluidPipeNetwork> networks;

        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return true;
        }

        public override void Start(ICoreAPI api)
        {
            base.Start(api);
        }

        public void AddNetwork()
        {

        }

        public void RemoveNetwork()
        {

        }

        public void ResetNetworks()
        {
            networks = new List<FluidPipeNetwork>();
        }
    }
}
