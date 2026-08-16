using UnityEngine;
using ZooWorld.Stats;
using ZooWorld.World;

namespace ZooWorld.Animals
{
    public interface IAnimalFactory
    {
        Animal Create(AnimalDefinition definition, Vector3 position);
    }

    /// <summary>
    /// The single place where an animal is assembled from species data and shared services.
    /// </summary>
    public sealed class AnimalFactory : IAnimalFactory
    {
        private readonly IWorldBounds _bounds;
        private readonly IFoodChainResolver _foodChain;
        private readonly IGameEvents _events;

        public AnimalFactory(IWorldBounds bounds, IFoodChainResolver foodChain, IGameEvents events)
        {
            _bounds = bounds;
            _foodChain = foodChain;
            _events = events;
        }

        public Animal Create(AnimalDefinition definition, Vector3 groundPosition)
        {
            var lift = definition.ViewPrefab.TryGetComponent(out Collider prefabCollider)
                ? prefabCollider.bounds.size.y
                : 0f;

            var instance = Object.Instantiate(
                definition.ViewPrefab,
                groundPosition + Vector3.up * lift,
                Quaternion.identity);
            instance.name = definition.Id;

            if (!instance.TryGetComponent(out Animal animal))
                animal = instance.AddComponent<Animal>();

            animal.Setup(definition, _bounds, _foodChain, _events, RandomDirection());
            return animal;
        }

        private static Vector3 RandomDirection()
        {
            var angle = Random.Range(0f, Mathf.PI * 2f);
            return new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
        }
    }
}
