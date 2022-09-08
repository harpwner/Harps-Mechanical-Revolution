using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarpTech.Mechanics.MechWorks;
using Vintagestory.API.Common;

namespace HarpTech.BEBehaviors
{
    class BEBehaviorMechAxle : BEMechBase
    {
        public BEBehaviorMechAxle(BlockEntity blockentity) : base(blockentity) { }

        public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
        {
            base.GetBlockInfo(forPlayer, dsc);

            dsc.AppendLine("HAHAHAHAHA");
        }
    }
}
