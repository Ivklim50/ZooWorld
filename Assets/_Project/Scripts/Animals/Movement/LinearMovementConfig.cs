using UnityEngine;

namespace ZooWorld.Animals.Movement
{
    [CreateAssetMenu(menuName = "Zoo/Movement/Linear", fileName = "LinearMovement")]
    public sealed class LinearMovementConfig : MovementConfig
    {
        [SerializeField, Min(0f)] private float _speed = 3f;

        public override IMovementBehaviour Create() => new LinearMovement(_speed);
    }
}
