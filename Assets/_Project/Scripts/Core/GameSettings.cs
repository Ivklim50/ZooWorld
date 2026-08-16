using UnityEngine;
using ZooWorld.Animals;

namespace ZooWorld.Core
{
    /// <summary>Game settings. Animals catalog and spawn parameters.</summary>
    [CreateAssetMenu(menuName = "Zoo/Game Settings", fileName = "GameSettings")]
    public sealed class GameSettings : ScriptableObject
    {
        [Header("Spawn")]
        [SerializeField, Min(0.1f)] private float _spawnIntervalMinSeconds = 1f;
        [SerializeField, Min(0.1f)] private float _spawnIntervalMaxSeconds = 2f;

        [Header("Catalog")]
        [Tooltip("Catalog of all animal species available in the game.")]
        [SerializeField] private AnimalDefinition[] _animals;

        public float SpawnIntervalMinSeconds => _spawnIntervalMinSeconds;
        public float SpawnIntervalMaxSeconds => _spawnIntervalMaxSeconds;
        public AnimalDefinition[] Animals => _animals;
    }
}