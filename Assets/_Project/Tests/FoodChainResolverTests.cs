using NUnit.Framework;
using ZooWorld.Animals;

namespace ZooWorld.Tests
{
    /// <summary>
    /// Food chain rules are verified without a scene, prefabs or the Unity runtime —
    /// that is exactly why the resolver is kept separate from MonoBehaviour.
    /// </summary>
    public sealed class FoodChainResolverTests
    {
        private static readonly DietProfile Frog =
            new(CreatureTag.Prey, CreatureTag.None);

        private static readonly DietProfile Snake =
            new(CreatureTag.Predator, CreatureTag.Prey | CreatureTag.Predator);

        /// <summary>Predator that hunts prey only and ignores other predators.</summary>
        private static readonly DietProfile Spider =
            new(CreatureTag.Predator, CreatureTag.Prey);

        /// <summary>Species that is neither prey nor predator to anyone.</summary>
        private static readonly DietProfile Untagged =
            new(CreatureTag.None, CreatureTag.None);

        /// <summary>Species that eats its own kind.</summary>
        private static readonly DietProfile Cannibal =
            new(CreatureTag.Prey, CreatureTag.Prey);

        private readonly IFoodChainResolver _resolver = new TagFoodChainResolver();

        [Test]
        public void Resolve_PreyVsPrey_ReturnsBounce()
            => Assert.AreEqual(InteractionResult.Bounce, _resolver.Resolve(Frog, Frog));

        [Test]
        public void Resolve_PredatorVsPrey_PredatorEats()
            => Assert.AreEqual(InteractionResult.AEatsB, _resolver.Resolve(Snake, Frog));

        [Test]
        public void Resolve_PreyVsPredator_PreyIsEaten()
            => Assert.AreEqual(InteractionResult.BEatsA, _resolver.Resolve(Frog, Snake));

        [Test]
        public void Resolve_PredatorVsSamePredator_ReturnsMutual()
            => Assert.AreEqual(InteractionResult.Mutual, _resolver.Resolve(Snake, Snake));

        [Test]
        public void Resolve_SelectivePredatorVsItsOwnKind_ReturnsBounce()
            => Assert.AreEqual(InteractionResult.Bounce, _resolver.Resolve(Spider, Spider));

        [Test]
        public void Resolve_OmnivoreVsSelectivePredator_OmnivoreEats()
            => Assert.AreEqual(InteractionResult.AEatsB, _resolver.Resolve(Snake, Spider));

        /// <summary>Swapping the arguments must mirror the outcome, not change it.</summary>
        [Test]
        public void Resolve_SelectivePredatorVsOmnivore_ResultIsMirrored()
            => Assert.AreEqual(InteractionResult.BEatsA, _resolver.Resolve(Spider, Snake));

        [Test]
        public void Resolve_UntaggedSpecies_IsNeverEaten()
            => Assert.AreEqual(InteractionResult.Bounce, _resolver.Resolve(Snake, Untagged));

        [Test]
        public void Resolve_CannibalVsItsOwnKind_ReturnsMutual()
            => Assert.AreEqual(InteractionResult.Mutual, _resolver.Resolve(Cannibal, Cannibal));
    }
}
