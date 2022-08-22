using HarpTech.BlockEntities;
using HarpTech.Enums;
using HarpTech.Interfaces;
using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace HarpTech.Mechanics
{
    class FluidBoiler
    {
        const float maxOutlet = 500;
        const float maxInlet = 400;

        public List<IFluidPipe> outlets;
        public List<IFluidPipe> inlets;
        public List<BEBoiler> boilers;
        public float water;
        public float steam;

        private float temperature;
        private float maxTemperature;
        public float boilRate;
        public float maxWater;
        private float heatEff;
        private float inRate = maxInlet;
        private float outRate = maxOutlet;
        private bool isHeated = false;
        public bool isValid = true;

        public long listenerID;

        ICoreAPI api;

        public FluidBoiler(ICoreAPI api)
        {
            this.api = api;
            maxTemperature = 900;
            maxWater = 0;
            heatEff = 0;
            water = 0;
            steam = 0;
            temperature = 0;
            boilRate = 0;
            outlets = new List<IFluidPipe>();
            inlets = new List<IFluidPipe>();
            boilers = new List<BEBoiler>();

            listenerID = api.World.RegisterGameTickListener(OnTick, 1000);
        }

        void OnTick(float why)
        {
            if (isHeated)
            {
                if(temperature < maxTemperature)
                {
                    temperature += (50 * heatEff);
                    if(temperature > maxTemperature) { temperature = maxTemperature; }
                }
            }
            else
            {
                if(temperature > 0)
                {
                    temperature -= 50;
                    if(temperature < 0) { temperature = 0; }
                }
            }

            if(temperature >= 100)
            {
                boilRate = (maxWater * temperature) / 10000;
            } else { boilRate = 0; }

            GetInlets();

            if(inlets != null)
            {
                foreach(IFluidPipe i in inlets)
                {
                    inRate = Math.Min(maxInlet, maxWater - water);
                    //water += i.RequestFluid(EnumFluidType.Water, inRate);
                }
            }
            
            if(water >= boilRate)
            {
                steam += boilRate;
                water -= boilRate;
            } else
            {
                steam += water;
                water = 0;
            }

            GetOutlets();

            if(outlets != null)
            {
                foreach(IFluidPipe i in outlets)
                {
                    outRate = Math.Min(maxOutlet, steam);
                    //i.AddFluid(EnumFluidType.Steam, outRate);
                }
            }
        }

        public void AddBoiler(BEBoiler boiler)
        {
            if(boilers == null) { boilers = new List<BEBoiler>(); }

            if(!boilers.Contains(boiler)) { boilers.Add(boiler); }
            RecalculateVolume();
        }

        public void RecalculateVolume()
        {
            if(boilers == null) { return; }

            maxWater = 0;

            foreach(BEBoiler i in boilers)
            {
                maxWater += 6000;
            }
        }

        public void toTree()
        {

        }

        public void FromTree()
        {

        }

        public void GetOutlets()
        {
            if(boilers == null) { return; }
            if(outlets == null) { outlets = new List<IFluidPipe>(); }

            foreach(BEBoiler i in boilers)
            {
                if (i.isOutlet)
                {
                    foreach (BlockFacing f in BlockFacing.ALLFACES)
                    {
                        BlockPos outPos = i.Pos.AddCopy(f);
                        IFluidPipe pipe = i.Api.World.BlockAccessor.GetBlockEntity(outPos) as IFluidPipe;
                        if(pipe != null && !outlets.Contains(pipe)) { outlets.Add(pipe); }
                    }
                }
            }
        }

        public void GetInlets()
        {
            if (boilers == null) { return; }
            if (inlets == null) { inlets = new List<IFluidPipe>(); }

            foreach (BEBoiler i in boilers)
            {
                if (!i.isOutlet)
                {
                    foreach (BlockFacing f in BlockFacing.ALLFACES)
                    {
                        BlockPos outPos = i.Pos.AddCopy(f);
                        IFluidPipe pipe = i.Api.World.BlockAccessor.GetBlockEntity(outPos) as IFluidPipe;
                        if (pipe != null && !inlets.Contains(pipe)) { inlets.Add(pipe); }
                    }
                }
            }
        }
    }
}
