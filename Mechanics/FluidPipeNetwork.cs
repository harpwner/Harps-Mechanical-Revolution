using HarpTech.Enums;
using HarpTech.Interfaces;
using System.Collections.Generic;

namespace HarpTech.Mechanics
{
    class FluidPipeNetwork
    {
        List<IFluidInput> producers;
        List<IFluidAcceptor> consumers;
    }
}
