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
    class PistonRenderer : IRenderer
    {
        public Matrixf ModelMat = new Matrixf();
        internal bool ShouldRender;

        ICoreClientAPI api;
        BlockPos pos;
        MeshRef meshRef;
        BEPiston piston;

        public PistonRenderer(ICoreClientAPI api, BlockPos pos, MeshData mesh, BEPiston piston)
        {
            this.api = api;
            this.pos = pos;
            this.piston = piston;
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

            float angle = 0;
            if(piston.lever != null)
            {
                //old eq: (float)(((1f / 3f) * piston.lever.pistonMod * Math.Sin(piston.lever.AngleRad)) + (1f / 3f));
                angle = ((6.55f / 16f) * piston.lever.pistonMod * (float)Math.Sin(piston.lever.AngleRad)) + (1f / 3f);
            }


            prog.ModelMatrix = ModelMat
                .Identity()
                .Translate(pos.X - camPos.X, pos.Y - camPos.Y, pos.Z - camPos.Z)
                .Translate(0, angle, 0)
                .Values
            ;

            prog.ViewMatrix = rpi.CameraMatrixOriginf;
            prog.ProjectionMatrix = rpi.CurrentProjectionMatrix;
            rpi.RenderMesh(meshRef);
            prog.Stop();
        }
    }
}
