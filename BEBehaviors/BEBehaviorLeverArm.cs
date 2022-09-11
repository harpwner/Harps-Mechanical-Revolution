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
        BlockFacing face;

        public float AngleRad => GetAngleRad();
        public float yRot;
        public int flywheelMod;
        public int pistonMod;

        public float Pressure => GetPressure();
        float lastTime;
        float dt;
        float fakeAngle;
        bool canPlay;
        bool engineComplete = false;

        public BEBehaviorLeverArm(BlockEntity blockentity) : base(blockentity) { }

        public override void Initialize(ICoreAPI api, JsonObject properties)
        {
            base.Initialize(api, properties);

            Blockentity.RegisterGameTickListener(TrySetupEngine, 1000);

            switch (this.Blockentity.Block.Variant["side"])
            {
                case "north":
                case "south":
                    yRot = 0;
                    face = BlockFacing.NORTH;
                    break;
                case "east":
                case "west":
                    yRot = 90;
                    face = BlockFacing.EAST;
                    break;
            }

            if (Api.Side == EnumAppSide.Client)
            {
                renderer = new LeverArmRenderer(Api as ICoreClientAPI, this.Blockentity.Pos, GetMesh(), this);
                renderer.ShouldRender = true;
                (Api as ICoreClientAPI).Event.RegisterRenderer(renderer, EnumRenderStage.Opaque, "wattleverarm");
            }
        }

        void TrySetupEngine(float dt)
        {
            BEBehaviorFlywheel flywheel = GetFlywheel();
            BEPiston piston = GetPiston();
            if(piston == null || flywheel == null) { engineComplete = false; return; }

            engineComplete = true;
        }

        MeshData GetMesh()
        {
            Block block = Api.World.BlockAccessor.GetBlock(this.Blockentity.Pos);
            if (block.BlockId == 0) return null;

            MeshData mesh;
            ITesselatorAPI mesher = ((ICoreClientAPI)Api).Tesselator;

            mesher.TesselateShape(block, Shape.TryGet(Api, "harptech:shapes/block/wattengine/lever_arm.json"), out mesh);

            return mesh;
        }

        public override void OnBlockRemoved()
        {
            renderer?.Dispose();
            renderer = null;
            if(piston != null) { piston.lever = null; }

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
                angle = flywheelMod * flywheel.AngleRad / 2;
            } else if(piston != null)
            {
                dt = Api.World.ElapsedMilliseconds - lastTime;
                lastTime = Api.World.ElapsedMilliseconds;

                float rotSpeed = Pressure * 80 * (dt / 1000) * GameMath.DEG2RAD;
                fakeAngle += rotSpeed;
                angle = fakeAngle;
            }

            if(Api.Side != EnumAppSide.Client) { return angle; }

            if (canPlay && piston != null && pistonMod * Math.Sin(angle) > 0.9f)
            {
                Api.World.PlaySoundAt(new AssetLocation("harptech:sounds/steam"), piston.Pos.X, piston.Pos.Y, piston.Pos.Z, null, true, 24, piston.Pressure * 2);
                canPlay = false;
            } else if (!canPlay)
            {
                canPlay = pistonMod * Math.Sin(angle) < -0.9f ? true : false;
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
            BlockFacing sideFacing = face.GetCW();
            BlockFacing sideOpposite = sideFacing.Opposite;
            BlockPos pos = this.Blockentity.Pos.Copy();
            BlockPos pos2 = pos.Copy();
            pos.Add(sideFacing).Add(sideFacing).Add(BlockFacing.DOWN).Add(BlockFacing.DOWN);
            pos2.Add(sideOpposite).Add(sideOpposite).Add(BlockFacing.DOWN).Add(BlockFacing.DOWN);

            flywheel = Api.World.BlockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorFlywheel>(); flywheelMod = -1;
            if(flywheel == null) { flywheel = Api.World.BlockAccessor.GetBlockEntity(pos2)?.GetBehavior<BEBehaviorFlywheel>(); flywheelMod = 1; }

            if(flywheel != null)
            {
                flywheelMod *= flywheel.GetRotDir();
            }

            return flywheel;
        }

        public BEPiston GetPiston()
        {
            BlockFacing sideFacing = face.GetCW();
            BlockFacing sideOpposite = sideFacing.Opposite;
            BlockPos pos = this.Blockentity.Pos.Copy();
            BlockPos pos2 = pos.Copy();
            pos.Add(sideFacing).Add(sideFacing).Add(BlockFacing.DOWN);
            pos2.Add(sideOpposite).Add(sideOpposite).Add(BlockFacing.DOWN); ;

            BlockEntity entity = Api.World.BlockAccessor.GetBlockEntity(pos); pistonMod = 1;
            if (entity == null || !(entity is BEPiston)) { entity = Api.World.BlockAccessor.GetBlockEntity(pos2); pistonMod = -1; }
            
            if(entity != null && entity is BEPiston) { piston = entity as BEPiston; }

            switch (face.Index)
            {
                //fixes the east/west piston inversion that occurs due to mechpower rotations
                case BlockFacing.indexEAST:
                    pistonMod *= -1;
                    break;
            }

            if(piston != null) { piston.lever = this; }

            return piston;
        }
    }
}
