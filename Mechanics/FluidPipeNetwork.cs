using HarpTech.Enums;
using HarpTech.Interfaces;
using System.Collections.Generic;

namespace HarpTech.Mechanics
{
    class FluidPipeNetwork
    {
        public List<IFluidPipe> network;
        public List<IFluidAcceptor> acceptors;
        public List<IFluidInput> inputs;

        public EnumFluidType fluidType;
        public float systemCapacity;
        public float systemDraw;

        public FluidPipeNetwork()
        {
            fluidType = EnumFluidType.None;
            network = new List<IFluidPipe>();
        }

        public void AddNode(IFluidPipe node)
        {
            if (network == null)
            {
                network = new List<IFluidPipe>();
            }
            network.Add(node);
        }

        public void RemoveNode(IFluidPipe node)
        {
            if (network == null)
            {
                return;
            }

            network.Remove(node);
        }

        public void AddInput(IFluidInput input)
        {
            if(inputs == null)
            {
                inputs = new List<IFluidInput>();
            }

            inputs.Add(input);
        }

        public void RemoveInput(IFluidInput input)
        {
            if(inputs == null)
            {
                return;
            }

            inputs.Remove(input);
        }

        public void AddAcceptor(IFluidAcceptor acceptor)
        {
            if(acceptors == null)
            {
                acceptors = new List<IFluidAcceptor>();
            }

            acceptors.Add(acceptor);
        }

        public void RemoveAcceptor(IFluidAcceptor acceptor)
        {
            if (acceptors == null)
            {
                return;
            }

            acceptors.Remove(acceptor);
        }

        public List<IFluidPipe> GetNetwork()
        {
            if (network == null)
            {
                return null;
            }

            return network;
        }

        public EnumFluidType GetFluidType()
        {
            return fluidType;
        }

        public bool HasNode(IFluidPipe node)
        {
            if (network == null)
            {
                return false;
            }

            return network.Contains(node);
        }

        public int GetSize()
        {
            if (network == null)
            {
                return 0;
            }

            return network.Count;
        }

        public void ClearNetwork()
        {
            network = new List<IFluidPipe>();
            inputs = new List<IFluidInput>();
            acceptors = new List<IFluidAcceptor>();
        }

        public float RequestFluid(EnumFluidType type, float amount)
        {
            float available = 0;



            return available;
        }

        /**public float GetTotalFluid()
        {
            float total = 0;

            if (network != null)
            {
                foreach (IFluidPipe i in network)
                {
                    total += i.GetFluid();
                }
            }

            fluidTotal = total;
            return total;
        }**/


        /**public void RedistributeFluid()
        {
            float totalFluid = 0;
            if(network == null) { return; }
            foreach(IFluidPipe i in network)
            {
                totalFluid += i.GetFluid();
                this.fluidType = i.GetFluidType();
            }

            float dist = totalFluid / network.Count;

            foreach(IFluidPipe i in network)
            {
                i?.SetFluid(fluidType, dist);
            }
        }**/

        /**public float AddFluid(EnumFluidType type, float amount)
        {
            if(fluidType == type || fluidType == EnumFluidType.None && network != null)
            {
                fluidType = type;
                float remaining = 0;
                float dist = amount / GetSize();
                foreach (IFluidPipe i in network) {
                    remaining += i.AddFluid(fluidType, dist);
                }

                return remaining;
            }

            return amount;
        }**/
    }
}
