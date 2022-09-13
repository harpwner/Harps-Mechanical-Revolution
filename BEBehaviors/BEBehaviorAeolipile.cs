using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using Vintagestory.GameContent.Mechanics;

namespace HarpTech.BEBehaviors
{
    class BEBehaviorAeolipile : BEBehaviorMPRotor
    {
        public static SimpleParticleProperties particles = new SimpleParticleProperties(1, 1, ColorUtil.ColorFromRgba(200, 200, 200, 50), new Vec3d(), new Vec3d(), new Vec3f(), new Vec3f());
        Vec3d steamPosition = new Vec3d(0, 0, 0);

        int sideAngle = 0;
        int waterContents = 0;
        float pressure;
        float temperature;
        double radius = 0.345d;

        protected int WaterContents => waterContents;
        protected override float Resistance => 0.8f;
        protected override double AccelerationFactor => 0.04;
        protected override float TargetSpeed => pressure * 2;
        protected override float TorqueFactor => pressure / 25;

        public BEBehaviorAeolipile(BlockEntity blockentity) : base(blockentity) { }

        public override void Initialize(ICoreAPI api, JsonObject properties)
        {
            base.Initialize(api, properties);
            InitParticles();
            Blockentity.RegisterGameTickListener(DoSteamStuff, 100);
            Blockentity.RegisterGameTickListener(ParticleEmit, 20);

            switch (Block.Variant["side"])
            {
                case "north":
                    sideAngle = 0;
                    break;
                case "south":
                    sideAngle = 180;
                    break;
                case "east":
                    sideAngle = 270;
                    break;
                case "west":
                    sideAngle = 90;
                    break;
            }
        }

        /// <summary>Gives the aeolipile particles their appearance.</summary>
        private void InitParticles()
        {
            steamPosition = new Vec3d(Position.X + 0.5, Position.Y + 0.5, Position.Z + 0.5);
            particles.MinSize = 0.05f;
            particles.MaxSize = 0.15f;
            particles.SizeEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEAR, 1.2f);
            particles.OpacityEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEAR, -50);
            particles.MinQuantity = 2;
            particles.LifeLength = 0.1f;
            particles.GravityEffect = 0;
            particles.ParticleModel = EnumParticleModel.Quad;
        }

        private void DoSteamStuff(float dt)
        {
            BlockPos pos = new BlockPos(Blockentity.Pos.X, Blockentity.Pos.Y - 1, Blockentity.Pos.Z);
            BlockEntity below = Api.World.BlockAccessor.GetBlockEntity(pos);
            float pitTemp = 0;

            if(below == null || !(below.Block is BlockFirepit)) { return; }

            BlockEntityFirepit firepit = (BlockEntityFirepit)below;
            if(firepit != null)
            {
                pitTemp = firepit.furnaceTemperature;
            }

            temperature = (temperature + pitTemp) / 2;
            if(temperature < 100)
            {
                pressure = 0;
            } else
            {
                pressure = Math.Min(1, waterContents) * (temperature / 900);
            }
            waterContents = (int)Math.Max(0, waterContents - (pressure));
            Blockentity.MarkDirty(true);
        }

        private void ParticleEmit(float dt)
        {
            if(Api.Side == EnumAppSide.Server) { return; }

            if(pressure == 0) { return; }

            double x, y = 0;
            float vx, vy = 0;

            x = radius * Math.Cos(AngleRad);
            y = radius * Math.Sin(AngleRad);

            vx = (float)x * pressure * 6;
            vy = (float)y * pressure * 6;


            particles.MinPos = new Vec3d(steamPosition.X, steamPosition.Y, steamPosition.Z);

            switch (sideAngle)
            {
                case 180:
                    particles.MinPos.X += x;
                    particles.MinPos.Y -= y;
                    particles.MinVelocity = new Vec3f((float)vy, (float)vx, 0);
                    Api.World.SpawnParticles(particles);

                    particles.MinPos = new Vec3d(steamPosition.X, steamPosition.Y, steamPosition.Z);
                    particles.MinPos.X -= x;
                    particles.MinPos.Y += y;
                    particles.MinVelocity = new Vec3f(-(float)vy, -(float)vx, 0);
                    Api.World.SpawnParticles(particles);
                    break;
                case 270:
                    particles.MinPos.Z += x;
                    particles.MinPos.Y += y;
                    particles.MinVelocity = new Vec3f(0, -(float)vx, (float)vy);
                    Api.World.SpawnParticles(particles);

                    particles.MinPos = new Vec3d(steamPosition.X, steamPosition.Y, steamPosition.Z);
                    particles.MinPos.Z -= x;
                    particles.MinPos.Y -= y;
                    particles.MinVelocity = new Vec3f(0, (float)vx, -(float)vy);
                    Api.World.SpawnParticles(particles);
                    break;
                case 0:
                    particles.MinPos.X -= x;
                    particles.MinPos.Y += y;
                    particles.MinVelocity = new Vec3f((float)vy, (float)vx, 0);
                    Api.World.SpawnParticles(particles);

                    particles.MinPos = new Vec3d(steamPosition.X, steamPosition.Y, steamPosition.Z);
                    particles.MinPos.X += x;
                    particles.MinPos.Y -= y;
                    particles.MinVelocity = new Vec3f(-(float)vy, -(float)vx, 0);
                    Api.World.SpawnParticles(particles);
                    break;
                case 90:
                    particles.MinPos.Z -= x;
                    particles.MinPos.Y -= y;
                    particles.MinVelocity = new Vec3f(0, -(float)vx, (float)vy);
                    Api.World.SpawnParticles(particles);

                    particles.MinPos = new Vec3d(steamPosition.X, steamPosition.Y, steamPosition.Z);
                    particles.MinPos.Z += x;
                    particles.MinPos.Y += y;
                    particles.MinVelocity = new Vec3f(0, (float)vx, -(float)vy);
                    Api.World.SpawnParticles(particles);
                    break;
            }
        }


        /// <summary>This method is used to attempt to grab water from a bucket and place it in the aeolipile.</summary>
        /// <param name="lc">The bucket held by the player.</param>
        /// <param name="byPlayer">The player.</param>
        /// <returns>Whether or not the operation was successful.</returns>
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

            Blockentity.MarkDirty(true);
            return true;
        }

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            tree.SetFloat("pressure", pressure);
            tree.SetFloat("temperature", temperature);
            tree.SetInt("water", waterContents);
            base.ToTreeAttributes(tree);
        }
        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
        {
            pressure = tree.GetFloat("pressure");
            temperature = tree.GetFloat("temperature");
            waterContents = tree.GetInt("water");
            base.FromTreeAttributes(tree, worldAccessForResolve);
        }

        public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
        {
            ICoreClientAPI capi = Api as ICoreClientAPI;

            if (capi == null)
            {
                return false;
            }

            Shape shape = capi.Assets.TryGet("harptech:shapes/block/aeolipile/aeolipile_bottom.json").ToObject<Shape>();

            MeshData mesh;
            capi.Tesselator.TesselateShape(Block, shape, out mesh, new Vec3f(0, sideAngle, 0));
            mesher.AddMeshData(mesh);

            return base.OnTesselation(mesher, tesselator);
        }

        public override void GetBlockInfo(IPlayer forPlayer, StringBuilder sb)
        {
            base.GetBlockInfo(forPlayer, sb);

            sb.AppendLine(string.Format("Temperature: {0}C", (int)temperature));
            sb.AppendLine(string.Format("Water Remaining: {0}mL", (int)waterContents));
        }
    }
}
