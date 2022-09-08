using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace HarpTech.BlockEntities
{
    class BEBoiler : BlockEntity
    {
        float waterContents;
        float temperature;
        float pressure;
        BlockEntityFirepit firepit;
        public float steamProduction;

        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);

            RegisterGameTickListener(Steam, 100);
        }

        void Steam(float dt)
        {
            if (firepit == null) { GetFirepit(); return; }

            float pitTemp = firepit.furnaceTemperature;

            temperature = (temperature + pitTemp) / 2;
            if (temperature < 100)
            {
                pressure = 0;
            }
            else
            {
                pressure = Math.Min(1, waterContents) * (temperature / 900) * dt;
            }
            waterContents = (int)Math.Max(0, waterContents - (pressure));
            steamProduction = pressure * 1700 / dt;
            MarkDirty(true);
        }

        void GetFirepit()
        {
            BlockPos below = this.Pos.DownCopy();
            Block belowBlock = Api.World.BlockAccessor.GetBlock(below);
            if(belowBlock != null & belowBlock is BlockFirepit)
            {
                firepit = Api.World.BlockAccessor.GetBlockEntity(below) as BlockEntityFirepit;
            }
        }

        public bool BucketInteract(BlockLiquidContainerBase lc, IPlayer byPlayer)
        {
            ItemStack containerStack = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack;

            if (lc == null || lc.GetContentProps(containerStack) == null) { return false; }

            ItemStack portions = lc.GetContent(containerStack);

            if (portions.Item != null && portions.Item.FirstCodePart().Equals("waterportion"))
            {
                if (waterContents <= 9000)
                {
                    if (lc.TryTakeContent(containerStack, 100) != null)
                    {
                        waterContents += 1000;
                    }
                    byPlayer.InventoryManager.ActiveHotbarSlot.MarkDirty();
                }
            }

            MarkDirty(true);
            return true;
        }

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            tree.SetFloat("temp", temperature);
            tree.SetFloat("water", waterContents);
            tree.SetFloat("steam", steamProduction);

            base.ToTreeAttributes(tree);
        }

        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
        {
            this.temperature = tree.GetFloat("temp");
            this.waterContents = tree.GetFloat("water");
            this.steamProduction = tree.GetFloat("steam");

            base.FromTreeAttributes(tree, worldAccessForResolve);
        }

        public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
        {
            base.GetBlockInfo(forPlayer, dsc);

            dsc.AppendLine("Temperature: " + temperature + "C");
            dsc.AppendLine("Water: " + waterContents + "mL");
            dsc.AppendLine("Steam Production: " + steamProduction + "mL/s");
        }
    }
}
