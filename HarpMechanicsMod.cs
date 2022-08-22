using Vintagestory.API.Common;
using HarpMechanics.Blocks;
using HarpMechanics.BEBehaviors;

namespace HarpMechanics
{
    class HarpMechanicsMod : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            api.RegisterBlockClass("BlockAeolipile", typeof(BlockAeolipile));
            api.RegisterBlockEntityBehaviorClass("BEBehaviorAeolipile", typeof(BEBehaviorAeolipile));
        }

    }
}
