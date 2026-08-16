using UnityEngine;
using ZooWorld.Animals.Movement;

namespace ZooWorld.Animals
{
    /// <summary>
    /// Species passport: a new species is a new asset, no code required.
    /// </summary>
    [CreateAssetMenu(menuName = "Zoo/Animal Definition", fileName = "AnimalDefinition")]
    public sealed class AnimalDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string _id = "animal";
        [SerializeField] private GameObject _viewPrefab;

        [Header("Behaviour")]
        [SerializeField] private MovementConfig _movement;

        [Header("Food chain")]
        [Tooltip("What this animal is to the others")]
        [SerializeField] private CreatureTag _traits = CreatureTag.Prey;
        [Tooltip("What this animal eats")]
        [SerializeField] private CreatureTag _eats = CreatureTag.None;

        [Header("Presentation")]
        [Tooltip("Label shown on a kill. Leave empty to show none")]
        [SerializeField] private string _killLabel = "Tasty!";

        public string Id => _id;
        public GameObject ViewPrefab => _viewPrefab;
        public MovementConfig Movement => _movement;
        public CreatureTag Traits => _traits;
        public DietProfile Diet => new(_traits, _eats);
        public string KillLabel => _killLabel;
    }
}
