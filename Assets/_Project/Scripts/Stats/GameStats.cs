using System;
using VContainer.Unity;
using ZooWorld.Animals;

namespace ZooWorld.Stats
{
    /// <summary>
    /// Death counters. Classification is driven by species tags, not by class type.
    /// </summary>
    public sealed class GameStats : IStartable, IDisposable
    {
        private readonly IGameEvents _events;

        public int DeadPrey { get; private set; }
        public int DeadPredators { get; private set; }

        public event Action Changed;

        public GameStats(IGameEvents events) => _events = events;

        public void Start() => _events.Died += OnDied;
        public void Dispose() => _events.Died -= OnDied;

        private void OnDied(Animal animal)
        {
            var traits = animal.Definition.Traits;

            if ((traits & CreatureTag.Predator) != 0) DeadPredators++;
            else if ((traits & CreatureTag.Prey) != 0) DeadPrey++;
            else return;   

            Changed?.Invoke();
        }
    }
}
