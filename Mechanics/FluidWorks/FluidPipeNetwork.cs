using HarpTech.BlockEntities;
using HarpTech.Enums;
using HarpTech.Interfaces;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace HarpTech.Mechanics
{
    class FluidPipeNetwork
    {
        const int tickMS = 5000;

        List<BEFluidProducerBase> producers = new List<BEFluidProducerBase>();
        List<BEFluidNetworkBase> components = new List<BEFluidNetworkBase>();
        List<BEFluidConsumerBase> consumers = new List<BEFluidConsumerBase>();
        List<BEFluidStorageBase> storage = new List<BEFluidStorageBase>();

        public EnumFluidType type = EnumFluidType.None;
        ICoreAPI api;
        public long listenerID;

        public FluidPipeNetwork(ICoreAPI api, BlockPos pos)
        {
            if(api.Side == EnumAppSide.Client) { return; }

            this.api = api;

            this.GenerateNetwork(pos);
            if(this.producers.Count == 0) { DeleteNetwork(); return; }
            EstablishPressure();
            SetType();

            listenerID = api.Event.RegisterGameTickListener(NetworkTick, tickMS);
            api.Logger.Chat("FP Network " + listenerID + "\tgenerated with " + components.Count + " components at position " + pos.ToVec3i() + " as a type " + type);
        }

        void NetworkTick(float dt)
        {
            api.Logger.Chat("FP Network " + listenerID + "\tticking after " + dt + " seconds with " + components.Count + " components");

            if (producers.Count == 0) { DeleteNetwork(); return; }

            SetType();

            float capacity = 0;

            foreach(BEFluidProducerBase p in producers)
            {
                capacity += (p.Production * dt);
                api.Logger.Chat("Vertical Stress of producer: " + p.verticalStress);
            }

            api.Logger.Chat("FP Network " + listenerID + "\tcapacity before: " + (capacity / dt));

            foreach (BEFluidConsumerBase c in consumers)
            {
                if (c.type == this.type) { capacity -= c.ConsumptionTick(dt, capacity); }
            }

            foreach(BEFluidStorageBase s in storage)
            {
                
            }

            api.Logger.Chat("FP Network " + listenerID + "\tcapacity after: " + (capacity / dt));
        }

        void SetType()
        {
            EnumFluidType type = EnumFluidType.None;
            int lowestStress = 0;

            if(producers == null || producers.Count == 0) { this.type = EnumFluidType.None; return; }

            BEFluidProducerBase prod = producers[0];
            type = prod.type;
            lowestStress = prod.verticalStress;

            if(producers.Count == 1) { this.type = type; return; }

            for(int i = 1; i < producers.Count; i++)
            {
                prod = producers[i];
                if(prod.verticalStress < lowestStress)
                {
                    type = ((BEFluidNetworkBase)prod).type;
                    lowestStress = prod.verticalStress;
                }
            }

            this.type = type;
            return;
        }

        void EstablishPressure()
        {
            foreach (BEFluidProducerBase p in producers)
            {
                p.verticalStress = 0;
                int yPos = p.Pos.Y;

                foreach (BEFluidNetworkBase n in components)
                {
                    int yDif = n.Pos.Y - p.Pos.Y;
                    p.verticalStress = GameMath.Max(yDif, p.verticalStress);
                }

                p.MarkDirty();
            }
        }

        public void Add(BEFluidNetworkBase be)
        {
            if (this.components.Contains(be)) { return; }

            this.components.Add(be);

            if (be is BEFluidProducerBase)
            {
                if (this.producers.Contains(be as BEFluidProducerBase)) { return; }
                this.producers.Add(be as BEFluidProducerBase);
            }

            if (be is BEFluidConsumerBase)
            {
                if (this.consumers.Contains(be as BEFluidConsumerBase)) { return; }
                this.consumers.Add(be as BEFluidConsumerBase);
            }

            be.SetNetwork(this);
        }

        public void Remove(BEFluidNetworkBase be)
        {
            this.components?.Remove(be);

            if (be is BEFluidProducerBase)
            {
                this.producers?.Remove(be as BEFluidProducerBase);
            }

            if (be is BEFluidConsumerBase)
            {
                this.consumers?.Remove(be as BEFluidConsumerBase);
            }
        }

        public void GenerateNetwork(BlockPos pos)
        {
            BEFluidNetworkBase be = api.World.BlockAccessor.GetBlockEntity(pos) as BEFluidNetworkBase;

            if (be == null || this.components.Contains(be)) { return; }

            this.Add(be);

            foreach(BlockPos p in be.connections)
            {
                BEFluidNetworkBase check = api.World.BlockAccessor.GetBlockEntity(p) as BEFluidNetworkBase;

                if(check != null && check.connections.Contains(pos))
                {
                    GenerateNetwork(p);
                }
            }
        }

        public int GetSize()
        {
            if(components == null) { return 0; }
            return this.components.Count;
        }

        public void NetworkReset()
        {
            this.producers = new List<BEFluidProducerBase>();
            this.consumers = new List<BEFluidConsumerBase>();
            this.components = new List<BEFluidNetworkBase>();
        }

        public void DeleteNetwork()
        {
            api.Logger.Chat("Deleting FP network " + listenerID);
            NetworkReset();
            api.Event.UnregisterGameTickListener(listenerID);
        }

        public void TransferNetwork(FluidPipeNetwork n)
        {
            if(n.listenerID == this.listenerID) { return; }

            foreach(BEFluidNetworkBase b in n.components)
            {
                Add(b);
            }

            n.DeleteNetwork();
        }
    }
}
