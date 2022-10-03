using HarpTech.BlockEntities;
using HarpTech.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace HarpTech.Mechanics
{
    class BEFluidNetworkBase : BlockEntity
    {
        public FluidPipeNetwork network;
        public EnumFluidType type;
        public List<BlockPos> connections;

        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);

            this.connections = GetConnections();

            if (Api != null && Api.Side == EnumAppSide.Server)
            {
                this.network = new FluidPipeNetwork(Api, Pos.Copy());
            }
        }

        public virtual List<BlockPos> GetConnections()
        {
            return new List<BlockPos>();
        }

        public override void OnBlockPlaced(ItemStack byItemStack = null)
        {
            base.OnBlockPlaced(byItemStack);

            if (Api.Side == EnumAppSide.Server)
            {
                this.network = new FluidPipeNetwork(Api, Pos.Copy());
            }
        }

        public override void OnBlockRemoved()
        {
            if(network != null)
            {
                network.Remove(this);
            }

            foreach (BlockPos p in connections)
            {
                BEFluidNetworkBase be = Api.World.BlockAccessor.GetBlockEntity(p) as BEFluidNetworkBase;
                if(be != null)
                {
                    be.network = new FluidPipeNetwork(Api, p.Copy());
                }
            }

            base.OnBlockRemoved();
        }

        public void SetNetwork(FluidPipeNetwork n)
        {
            if(this.network != null)
            {
                this.network.Remove(this);
            }

            this.network = n;
        }

        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
        {
            base.FromTreeAttributes(tree, worldAccessForResolve);

            this.connections = GetConnections();
        }

        public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
        {
            base.GetBlockInfo(forPlayer, dsc);

            if (network == null) { return; }
            dsc.AppendLine("Network size: " + this.network.GetSize());
        }
    }
}
