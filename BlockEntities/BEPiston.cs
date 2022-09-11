using HarpTech.BEBehaviors;
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

namespace HarpTech.BlockEntities
{
    class BEPiston : BlockEntity
    {
        public float Pressure => GetPressure();
        public BEBehaviorLeverArm lever;
        PistonRenderer renderer;

        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);

            if (api.Side == EnumAppSide.Client)
            {
                renderer = new PistonRenderer(api as ICoreClientAPI, this.Pos, GetMesh(), this);
                renderer.ShouldRender = true;
                (api as ICoreClientAPI).Event.RegisterRenderer(renderer, EnumRenderStage.Opaque, "wattpiston");
            }
        }

        MeshData GetMesh()
        {
            Block block = Api.World.BlockAccessor.GetBlock(this.Pos);
            if (block.BlockId == 0) return null;

            MeshData mesh;
            ITesselatorAPI mesher = ((ICoreClientAPI)Api).Tesselator;

            mesher.TesselateShape(block, Shape.TryGet(Api, "harptech:shapes/block/wattengine/piston_arm.json"), out mesh);

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

        float GetPressure()
        {
            BEBoiler boiler = GetBoiler();
            if(boiler == null) { return 0; }

            return boiler.steamProduction / 1700;
        }

        BEBoiler GetBoiler()
        {
            return Api.World.BlockAccessor.GetBlockEntity(Pos.DownCopy()) as BEBoiler;
        }   

        public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
        {
            base.GetBlockInfo(forPlayer, dsc);

            dsc.AppendLine("Pressure: " + Pressure);
        }
    }
}
