using Vintagestory.API.Common;
using HarpTech.Blocks;
using HarpTech.BlockEntities;
using HarpTech.BEBehaviors;

namespace HarpTech
{
    class HarpTechMod : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            api.RegisterBlockClass("BlockTurbine", typeof(BlockTurbine));
            api.RegisterBlockClass("BlockBellows", typeof(BlockBellows));
            api.RegisterBlockClass("BlockFluidPipe", typeof(BlockFluidPipe));
            api.RegisterBlockClass("BlockWaterPump", typeof(BlockWaterPump));
            api.RegisterBlockClass("BlockFireboxSmall", typeof(BlockFireboxSmall));
            api.RegisterBlockClass("BlockSmallBoiler", typeof(BlockSmallBoiler));
            api.RegisterBlockClass("BlockFlywheel", typeof(BlockFlywheel));
            api.RegisterBlockClass("BlockAeolipile", typeof(BlockAeolipile));

            api.RegisterBlockEntityClass("BEFluidPipe", typeof(BEFluidPipe));
            api.RegisterBlockEntityClass("BEBellows", typeof(BEBellows));
            api.RegisterBlockEntityClass("BEHeatingElement", typeof(BEHeatingElement));
            api.RegisterBlockEntityClass("BEFurnace", typeof(BEFurnace));
            api.RegisterBlockEntityClass("BEWaterPump", typeof(BEWaterPump));
            api.RegisterBlockEntityClass("BEBoiler", typeof(BEBoiler));
            api.RegisterBlockEntityClass("BEFireboxSmall", typeof(BEFireboxSmall));
            api.RegisterBlockEntityClass("BESmallBoiler", typeof(BESmallBoiler));
            api.RegisterBlockEntityClass("BEPiston", typeof(BEPiston));

            api.RegisterBlockEntityBehaviorClass("BEBehaviorTurbine", typeof(BEBehaviorTurbine));
            api.RegisterBlockEntityBehaviorClass("BEBehaviorWaterPump", typeof(BEBehaviorWaterPump));
            api.RegisterBlockEntityBehaviorClass("BEBehaviorFlywheel", typeof(BEBehaviorFlywheel));
            api.RegisterBlockEntityBehaviorClass("BEBehaviorAeolipile", typeof(BEBehaviorAeolipile));
        }

    }
}
