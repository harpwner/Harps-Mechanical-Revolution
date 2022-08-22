using HarpTech.Mechanics;
using HarpTech.Enums;

namespace HarpTech.Interfaces
{
    interface IFluidPipe
    {
        /// <summary>
        /// Retrieves the amount of fluid contained in the pipe, but does not return what type.
        /// </summary>
        /// <returns>The amount of fluid this pipe contains.</returns>
        ///float GetFluid();

        /// <summary>
        /// Retrieves the type of fluid contained within the pipe, but not the amount.
        /// </summary>
        /// <returns>EnumFluidType defining the type of fluid the pipe contains.</returns>
        EnumFluidType GetFluidType();

        /// <summary>
        /// Attempts to add a certain amount of a certain type of fluid to a pipe. Used by the network, use InputFluid instead for adding to the system externally.
        /// </summary>
        /// <param name="type">The fluid type to add.</param>
        /// <param name="fluid">The amount of fluid to add.</param>
        /// <returns>The fluid remaining after adding, if the pipe is full.</returns>
        ///float AddFluid(EnumFluidType type, float fluid);

        /// <summary>
        /// Sets the fluid type and amount for this pipe, overriding any previous fluid.
        /// </summary>
        /// <param name="type">The fluid type to set to.</param>
        /// <param name="fluid">The fluid amount to set to.</param>
        ///void SetFluid(EnumFluidType type, float fluid);

        /// <summary>
        /// Attempts to input a certain amount of fluid of a certain type to a pipe. focused on an individual pipe.
        /// </summary>
        /// <param name="type">The fluid type to set to.</param>
        /// <param name="fluid">The fluid amount to set to.</param>
        /// <returns>The fluid remaining after inputting, if the pipe is full.</returns>
        ///float InputFluid(EnumFluidType type, float fluid); 

        /// <summary>
        /// Asks the pipe for a certain fluid of a certain type.
        /// </summary>
        /// <param name="type">The fluid type to set to.</param>
        /// <param name="amount">The fluid amount to set to.</param>
        /// <returns>The amount of fluid the pipe can spare, if the type matches.</returns>
        ///float RequestFluid(EnumFluidType type, float amount);

        /// <summary>
        /// A recursive method that creates a new pipe system. Called frequently to keep the system fresh.
        /// </summary>
        /// <param name="network">The new network being created, null to create a brand new network.</param>
        void CreateNewNetwork(FluidPipeNetwork network = null);

        /// <summary>
        /// Gets the network that this pipe is a part of.
        /// </summary>
        /// <returns>The pipe network that this pipe has.</returns>
        FluidPipeNetwork GetNetwork();

        /// <summary>
        /// Checks for a pipe's validity. An invalid pipe is prevented from being attached to a network.
        /// </summary>
        /// <returns>Whether this is a valid pipe.</returns>
        bool Valid();
    }
}