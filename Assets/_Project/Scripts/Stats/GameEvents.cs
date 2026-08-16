using System;
using ZooWorld.Animals;

namespace ZooWorld.Stats
{
    /// <summary>
    /// Game event bus. Decouples animals, statistics and UI.
    /// </summary>
    public interface IGameEvents
    {
        event Action<Animal> Died;
        event Action<Animal, Animal> Killed; // (killer, victim)

        void RaiseDied(Animal animal);
        void RaiseKilled(Animal killer, Animal victim);
    }

    public sealed class GameEvents : IGameEvents
    {
        public event Action<Animal> Died;
        public event Action<Animal, Animal> Killed;

        public void RaiseDied(Animal animal) => Died?.Invoke(animal);
        public void RaiseKilled(Animal killer, Animal victim) => Killed?.Invoke(killer, victim);
    }
}
