using Vintagestory.API.Common;
using HarpTech.Blocks;
using HarpTech.BEBehaviors;

namespace HarpTech
{
    class HarpTech : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            api.RegisterBlockClass("BlockAeolipile", typeof(BlockAeolipile));
            api.RegisterBlockEntityBehaviorClass("BEBehaviorAeolipile", typeof(BEBehaviorAeolipile));
        }
    }
}
