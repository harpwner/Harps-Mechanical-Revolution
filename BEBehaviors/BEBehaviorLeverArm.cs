using HarpTech.BlockEntities;
using HarpTech.Mechanics;
using HarpTech.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace HarpTech.BEBehaviors
{
    class BEBehaviorLeverArm : BlockEntityBehavior
    {
        BEBehaviorFlywheel flywheel;
        BEPiston piston;
        LeverArmRenderer renderer;

        public float AngleRad => GetAngleRad();
        public float yRot;
        public float Pressure => GetPressure();
        float systemPressure;
        float lastTime;
        float dt;
        float fakeAngle;
        bool engineComplete = false;

        public BEBehaviorLeverArm(BlockEntity blockentity) : base(blockentity) { }

        public override void Initialize(ICoreAPI api, JsonObject properties)
        {
            base.Initialize(api, properties);

            Blockentity.RegisterGameTickListener(TrySetupEngine, 1000);

            switch (this.Blockentity.Block.Variant["side"])
            {
                case "north":
                    yRot = 0;
                    break;
                case "south":
                    yRot = 180;
                    break;
                case "east":
                    yRot = 270;
                    break;
                case "west":
                    yRot = 90;
                    break;
            }

            if (api.Side == EnumAppSide.Client)
            {
                renderer = new LeverArmRenderer(api as ICoreClientAPI, this.Blockentity.Pos, GetMesh(), this);
                renderer.ShouldRender = true;
                (api as ICoreClientAPI).Event.RegisterRenderer(renderer, EnumRenderStage.Opaque, "wattleverarm");
            }
        }

        void TrySetupEngine(float dt)
        {
            if(GetFlywheel() == null || GetPiston() == null) { engineComplete = false; return; }

            engineComplete = true;
            piston.lever = this;
        }

        MeshData GetMesh()
        {
            Block block = Api.World.BlockAccessor.GetBlock(this.Blockentity.Pos);
            if (block.BlockId == 0) return null;

            MeshData mesh;
            ITesselatorAPI mesher = ((ICoreClientAPI)Api).Tesselator;

            mesher.TesselateShape(block, Vintagestory.API.Common.Shape.TryGet(Api, "harptech:shapes/block/wattengine/lever_top.json"), out mesh);

            return mesh;
        }
        public override void OnBlockRemoved()
        {
            renderer?.Dispose();
            renderer = null;

            base.OnBlockRemoved();
        }

        public override void OnBlockUnloaded()
        {
            renderer?.Dispose();
            renderer = null;

            base.OnBlockUnloaded();
        }

        float GetAngleRad()
        {
            float angle = 0;

            if(flywheel != null)
            {
                angle = flywheel.AngleRad * GameMath.RAD2DEG / 2;
            } else if(piston != null)
            {
                dt = Api.World.ElapsedMilliseconds - lastTime;
                lastTime = Api.World.ElapsedMilliseconds;

                float rotSpeed = Pressure * 80 * (dt / 1000);
                return fakeAngle += rotSpeed;
            }

            return angle;
        }

        float GetPressure()
        {
            if(piston != null) { return piston.Pressure; }
            return 0;
        }

        public BEBehaviorFlywheel GetFlywheel()
        {
            BlockFacing sideFacing = BlockFacing.FromCode(this.Blockentity.Block.Variant["side"]).GetCW();
            BlockFacing sideOpposite = sideFacing.Opposite;
            BlockPos pos = this.Blockentity.Pos.Copy();
            BlockPos pos2 = pos.Copy();
            pos.Add(sideFacing).Add(sideFacing).Add(BlockFacing.DOWN).Add(BlockFacing.DOWN);
            pos2.Add(sideOpposite).Add(sideOpposite).Add(BlockFacing.DOWN).Add(BlockFacing.DOWN);

            flywheel = Api.World.BlockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorFlywheel>();
            if(flywheel == null) { flywheel = Api.World.BlockAccessor.GetBlockEntity(pos2)?.GetBehavior<BEBehaviorFlywheel>(); }

            return flywheel;
        }

        public BEPiston GetPiston()
        {
            BlockFacing sideFacing = BlockFacing.FromCode(this.Blockentity.Block.Variant["side"]).GetCW();
            BlockFacing sideOpposite = sideFacing.Opposite;
            BlockPos pos = this.Blockentity.Pos.Copy();
            BlockPos pos2 = pos.Copy();
            pos.Add(sideFacing).Add(sideFacing).Add(BlockFacing.DOWN);
            pos2.Add(sideOpposite).Add(sideOpposite).Add(BlockFacing.DOWN); ;

            BlockEntity entity = Api.World.BlockAccessor.GetBlockEntity(pos);
            if(entity == null) { entity = Api.World.BlockAccessor.GetBlockEntity(pos2); }
            
            if(entity != null && entity is BEPiston) { piston = entity as BEPiston; }
            else { piston = null; return null; }

            return piston;
        }

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            tree.SetFloat("eff", systemPressure);

            base.ToTreeAttributes(tree);
        }

        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
        {
            this.systemPressure = tree.GetFloat("eff");

            base.FromTreeAttributes(tree, worldAccessForResolve);
        }

        public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
        {
            base.GetBlockInfo(forPlayer, dsc);

            dsc.AppendLine("Engine Complete?: " + engineComplete);
            dsc.AppendLine("angle: " + AngleRad);
        }
    }
}
