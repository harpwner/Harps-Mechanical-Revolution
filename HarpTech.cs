using Vintagestory.API.Common;
using HarpTech.Blocks;
using HarpTech.BEBehaviors;
using HarpTech.BlockEntities;

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
            api.RegisterBlockClass("BlockBoiler", typeof(BlockBoiler));
            api.RegisterBlockClass("BlockPiston", typeof(BlockPiston));

            api.RegisterBlockEntityClass("BESmallBoiler", typeof(BEBoiler));
            api.RegisterBlockEntityClass("BEPiston", typeof(BEPiston));
            api.RegisterBlockEntityClass("BEFakeBlock", typeof(BEFakeBlock));

            api.RegisterBlockEntityBehaviorClass("BEBehaviorAeolipile", typeof(BEBehaviorAeolipile));
            api.RegisterBlockEntityBehaviorClass("BEBehaviorLeverArm", typeof(BEBehaviorLeverArm));
            api.RegisterBlockEntityBehaviorClass("BEBehaviorFlywheel", typeof(BEBehaviorFlywheel));
        }
    }
}
