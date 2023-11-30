using HarpTech.BEBehaviors;
using HarpTech.BlockEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace HarpTech.Renderers
{
    class FlywheelGearRenderer : IRenderer
    {
        public Matrixf ModelMat = new Matrixf();
        internal bool ShouldRender;

        ICoreClientAPI api;
        BlockPos pos;
        MeshRef meshRef;
        BEBehaviorFlywheel flywheel;

        public FlywheelGearRenderer(ICoreClientAPI api, BlockPos pos, MeshData mesh, BEBehaviorFlywheel flywheel)
        {
            this.api = api;
            this.pos = pos;
            this.flywheel = flywheel;
            meshRef = api.Render.UploadMesh(mesh);
        }

        public double RenderOrder => 0.5;

        public int RenderRange => 24;

        public void Dispose()
        {
            api.Event.UnregisterRenderer(this, EnumRenderStage.Opaque);

            meshRef.Dispose();
        }

        public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
        {
            if (meshRef == null || !ShouldRender) return;

            IRenderAPI rpi = api.Render;
            Vec3d camPos = api.World.Player.Entity.CameraPos;

            rpi.GlDisableCullFace();
            rpi.GlToggleBlend(true);

            IStandardShaderProgram prog = rpi.PreparedStandardShader(pos.X, pos.Y, pos.Z);
            prog.Tex2D = api.BlockTextureAtlas.AtlasTextures[0].TextureId;

            double translateCos = 6.1f / 16f * Math.Cos(flywheel.AngleRad / 2);
            double translateSin = 6.1f / 16f * Math.Sin(flywheel.AngleRad / 2);
            int[] modifier = flywheel.GetRendModifiers();

            prog.ModelMatrix = ModelMat
                .Identity()
                .Translate(pos.X - camPos.X, pos.Y - camPos.Y, pos.Z - camPos.Z)
                .Translate(translateCos * modifier[0], translateSin * modifier[1], translateCos * modifier[2])
                .Translate(0.5f, 0.5f, 0.5f)
                .RotateY(modifier[3] * GameMath.DEG2RAD)
                .Translate(-0.5f, -0.5f, -0.5f)
                .Values
            ;

            prog.ViewMatrix = rpi.CameraMatrixOriginf;
            prog.ProjectionMatrix = rpi.CurrentProjectionMatrix;
            rpi.RenderMesh(meshRef);
            prog.Stop();
        }
    }
}
