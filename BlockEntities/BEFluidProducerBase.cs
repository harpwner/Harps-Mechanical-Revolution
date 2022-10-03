using HarpTech.Interfaces;
using HarpTech.Mechanics;
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
    class BEFluidProducerBase : BEFluidNetworkBase
    {
        public virtual float Production => 50;

        /// <value>Difference in height between the producer and highest system component.</value>
        public int verticalStress = 0;

        public override void Initialize(ICoreAPI api)
        {
            this.type = Enums.EnumFluidType.Water;
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

        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
        {
            this.verticalStress = tree.GetInt("stress");

            base.FromTreeAttributes(tree, worldAccessForResolve);
        }

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            base.ToTreeAttributes(tree);

            tree.SetInt("stress", verticalStress);
        }
    }
}
