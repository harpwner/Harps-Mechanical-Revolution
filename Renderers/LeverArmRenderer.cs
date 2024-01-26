using HarpTech.BEBehaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace HarpTech.Renderers
{
    class LeverArmRenderer : IRenderer
    {
        public Matrixf ModelMat = new Matrixf();

        internal bool ShouldRender;

        ICoreClientAPI api;
        BlockPos pos;
        MeshRef meshRef;
        BEBehaviorLeverArm leverArm;

        public LeverArmRenderer(ICoreClientAPI api, BlockPos pos, MeshData mesh, BEBehaviorLeverArm leverArm)
        {
            this.api = api;
            this.pos = pos;
            this.leverArm = leverArm;
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

            float armAngle = (float)Math.Asin((5.515f / 32f) * Math.Sin(leverArm.AngleRad));

            prog.ModelMatrix = ModelMat
                .Identity()
                .Translate(pos.X - camPos.X, pos.Y - camPos.Y, pos.Z - camPos.Z)
                .Translate(0.5f, 0.5f, 0.5f)
                .RotateY(leverArm.yRot * GameMath.DEG2RAD)
                .RotateZ(armAngle)
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
