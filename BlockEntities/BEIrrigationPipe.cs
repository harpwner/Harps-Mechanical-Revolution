using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace HarpTech.BlockEntities
{
    class BEIrrigationPipe : BEFluidConsumerBase
    {
        const int verticalCheck = 8;

        public override float Consumption => 1f;

        public override void Initialize(ICoreAPI api)
        {
            this.type = Enums.EnumFluidType.Water;
            base.Initialize(api);
        }

        public override List<BlockPos> GetConnections()
        {
            List<BlockPos> connections = new List<BlockPos>();

            BlockFacing facing = BlockFacing.FromCode(Block.Variant["side"]);
            switch (facing.Index)
            {
                case BlockFacing.indexNORTH:
                case BlockFacing.indexSOUTH:
                    connections.Add(Pos.NorthCopy());
                    connections.Add(Pos.SouthCopy());
                    break;
                case BlockFacing.indexEAST:
                case BlockFacing.indexWEST:
                    connections.Add(Pos.EastCopy());
                    connections.Add(Pos.WestCopy());
                    break;
            }

            return connections;    
        }

        public override float ConsumptionTick(float dt, float capacity)
        {
            float consumption = base.ConsumptionTick(dt, capacity);

            Water(dt * efficiency / 100);

            return consumption;
        }

        void Water(float dt)
        {
            BlockPos pos = this.Pos.DownCopy();

            WaterLand(dt, pos.Copy());
            WaterLand(dt, pos.NorthCopy());
            WaterLand(dt, pos.SouthCopy());
            WaterLand(dt, pos.EastCopy());
            WaterLand(dt, pos.WestCopy());
        }

        void WaterLand(float dt, BlockPos startPos)
        {
            bool finished = false;
            for (int i = 0; i < verticalCheck && !finished; i++)
            {
                if (Api.World.BlockAccessor.GetBlock(startPos).Id != 0)
                {
                    finished = true;
                    BlockEntityFarmland be = Api.World.BlockAccessor.GetBlockEntity(startPos) as BlockEntityFarmland;
                    if (be != null)
                    {
                        be.WaterFarmland(dt, false);
                        be.MarkDirty();
                    }
                }

                startPos.Down();
            }
        }
    }
}
