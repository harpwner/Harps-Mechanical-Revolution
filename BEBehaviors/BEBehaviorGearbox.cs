using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent.Mechanics;

namespace HarpTech.BEBehaviors
{
    class BEBehaviorGearbox : BEBehaviorMPBase
    {
        BlockFacing outFace = BlockFacing.NORTH;
        float gearRatio;

        public BEBehaviorGearbox(BlockEntity blockentity) : base(blockentity) { }

        public override float GetResistance()
        {
            return 0.0005f;
        }

        public override void Initialize(ICoreAPI api, JsonObject properties)
        {
            outFace = BlockFacing.FromCode(Block.Variant["side"]);

            base.Initialize(api, properties);

            switch (Block.Variant["side"])
            {
                case "north":
                    this.AxisSign = new int[3] { 0, 0, -1 };
                    break;
                case "south":
                    this.AxisSign = new int[3] { 0, 0, -1 };
                    break;
                case "east":
                    this.AxisSign = new int[3] { -1, 0, 0 };
                    break;
                case "west":
                    this.AxisSign = new int[3] { -1, 0, 0 };
                    break;
            }

            switch (Block.Variant["ratio"])
            {
                case "1":
                    this.gearRatio = -1;
                    break;
                case "2":
                    this.gearRatio = 2f;
                    break;
                case "4":
                    this.gearRatio = 4f;
                    break;
                case "8":
                    this.gearRatio = 8;
                    break;
            }
        }

        public override bool IsPropagationDirection(BlockPos fromPos, BlockFacing test)
        {
            if (propagationDir == outFace || propagationDir == outFace.Opposite) return true;

            return false;
        }

        /*protected override MechPowerPath[] GetMechPowerExits(MechPowerPath pathDir)
        {
            MechPowerPath[] pathss = new MechPowerPath[1];
            bool invert = pathDir.IsInvertedTowards(Position);
            pathss[0] = new MechPowerPath(outFace.Opposite, pathDir.gearingRatio / gearRatio, null, !invert);
            pathss[0] = new MechPowerPath(outFace, pathDir.gearingRatio * gearRatio, null, invert);
            return pathss;
        }*/


        public override void SetPropagationDirection(MechPowerPath path)
        {
            BlockFacing turnDir = path.NetworkDir();

            if (turnDir == outFace)
            {
                this.GearedRatio = path.gearingRatio / gearRatio;
            }
            else
            {
                this.GearedRatio = path.gearingRatio * gearRatio;
            }

            this.propagationDir = turnDir;
        }

        /*public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
        {
            ICoreClientAPI capi = Api as ICoreClientAPI;

            if (capi == null)
            {
                return false;
            }

            Shape shape = capi.Assets.TryGet("harptech:shapes/block/gearbox/gearbox_out.json").ToObject<Shape>();

            MeshData mesh;

            capi.Tesselator.TesselateShape(Block, shape, out mesh, new Vec3f(outX * GearedRatio * AngleRad * (float)(180 / Math.PI), sideAngle, outZ * GearedRatio * AngleRad * (float)(180 / Math.PI)));
            mesher.AddMeshData(mesh);

            return base.OnTesselation(mesher, tesselator);
        }*/
    }
}
