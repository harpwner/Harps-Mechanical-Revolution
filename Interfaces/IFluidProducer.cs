using System.Collections.Generic;
using Vintagestory.API.MathTools;
using HarpTech.Enums;

namespace HarpTech.Interfaces
{
    interface IFluidProducer
    {
        float GetProduction();

        int GetVerticalStress();
    }
}
