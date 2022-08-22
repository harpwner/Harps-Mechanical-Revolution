using System.Collections.Generic;
using Vintagestory.API.MathTools;
using HarpTech.Enums;

namespace HarpTech.Interfaces
{
    interface IFluidInput
    {
        void SendFluid(EnumFluidType type, float amount);
        List<IFluidPipe> GetNeighboringPipes();
        BlockFacing GetFacing();
    }
}
