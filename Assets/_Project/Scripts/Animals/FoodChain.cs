using System;

namespace ZooWorld.Animals
{
    /// <summary>
    /// What an animal is and what it eats.
    /// </summary>
    [Flags]
    public enum CreatureTag
    {
        None = 0,
        Prey = 1 << 0,
        Predator = 1 << 1,
    }

    public readonly struct DietProfile
    {
        public readonly CreatureTag Traits; // what I am
        public readonly CreatureTag Eats;   // what I eat

        public DietProfile(CreatureTag traits, CreatureTag eats)
        {
            Traits = traits;
            Eats = eats;
        }
    }

    public enum InteractionResult
    {
        Bounce,
        AEatsB,
        BEatsA,
        /// <summary>Both can eat each other — one survives; the caller decides which.</summary>
        Mutual
    }

    public interface IFoodChainResolver
    {
        InteractionResult Resolve(in DietProfile a, in DietProfile b);
    }

    public sealed class TagFoodChainResolver : IFoodChainResolver
    {
        public InteractionResult Resolve(in DietProfile a, in DietProfile b)
        {
            var aEats = (b.Traits & a.Eats) != 0;
            var bEats = (a.Traits & b.Eats) != 0;

            if (aEats && bEats) return InteractionResult.Mutual;
            if (aEats) return InteractionResult.AEatsB;
            if (bEats) return InteractionResult.BEatsA;
            return InteractionResult.Bounce;
        }
    }
}
