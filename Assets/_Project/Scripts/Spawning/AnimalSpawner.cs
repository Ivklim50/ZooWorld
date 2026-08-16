using UnityEngine;
using VContainer.Unity;
using ZooWorld.Animals;
using ZooWorld.Core;
using ZooWorld.World;

namespace ZooWorld.Spawning
{
    /// <summary>
    /// Spawns one animal of a random species from the catalog.
    /// Knows nothing about concrete species — only about the list of AnimalDefinition assets.
    /// </summary>
    public sealed class AnimalSpawner : ITickable
    {
        private readonly GameSettings _settings;
        private readonly IAnimalFactory _factory;
        private readonly IWorldBounds _bounds;

        private float _timer;

        public AnimalSpawner(GameSettings settings, IAnimalFactory factory, IWorldBounds bounds)
        {
            _settings = settings;
            _factory = factory;
            _bounds = bounds;
            _timer = NextInterval();
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;
            if (_timer > 0f) return;

            _timer = NextInterval();
            Spawn();
        }

        private void Spawn()
        {
            var catalog = _settings.Animals;
            if (catalog == null || catalog.Length == 0) return;

            var definition = catalog[Random.Range(0, catalog.Length)];
            _factory.Create(definition, _bounds.RandomPointInside());
        }

        private float NextInterval()
            => Random.Range(_settings.SpawnIntervalMinSeconds, _settings.SpawnIntervalMaxSeconds);
    }
}
