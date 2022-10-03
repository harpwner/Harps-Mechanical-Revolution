using Vintagestory.API.Common;
using HarpTech.Blocks;
using HarpTech.BEBehaviors;
using HarpTech.BlockEntities;
using HarpTech.Mechanics;

namespace HarpTech 
{ 
    class HarpTech : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            api.RegisterBlockClass("BlockAeolipile", typeof(BlockAeolipile));
            api.RegisterBlockClass("BlockLeverArm", typeof(BlockLeverArm));
            api.RegisterBlockClass("BlockFlywheel", typeof(BlockFlywheel));
            api.RegisterBlockClass("HarpBlockBoiler", typeof(BlockBoiler));
            api.RegisterBlockClass("BlockPiston", typeof(BlockPiston));
            api.RegisterBlockClass("BlockFluidPipe", typeof(BlockFluidPipe));

            api.RegisterBlockEntityClass("BESmallBoiler", typeof(BEBoiler));
            api.RegisterBlockEntityClass("BEPiston", typeof(BEPiston));
            api.RegisterBlockEntityClass("BEFakeBlock", typeof(BEFakeBlock));
            api.RegisterBlockEntityClass("BEFluidTransporter", typeof(BEFluidTransporter));
            api.RegisterBlockEntityClass("BEFluidNetworkBase", typeof(BEFluidNetworkBase));
            api.RegisterBlockEntityClass("BEFluidProducerBase", typeof(BEFluidProducerBase));
            api.RegisterBlockEntityClass("BEFluidConsumerBase", typeof(BEFluidConsumerBase));
            api.RegisterBlockEntityClass("BEIrrigationPipe", typeof(BEIrrigationPipe));

            api.RegisterBlockEntityBehaviorClass("BEBehaviorAeolipile", typeof(BEBehaviorAeolipile));
            api.RegisterBlockEntityBehaviorClass("BEBehaviorLeverArm", typeof(BEBehaviorLeverArm));
            api.RegisterBlockEntityBehaviorClass("BEBehaviorFlywheel", typeof(BEBehaviorFlywheel));
        }
    }
}
