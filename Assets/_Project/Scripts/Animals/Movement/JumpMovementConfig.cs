using UnityEngine;

namespace ZooWorld.Animals.Movement
{
    [CreateAssetMenu(menuName = "Zoo/Movement/Jump", fileName = "JumpMovement")]
    public sealed class JumpMovementConfig : MovementConfig
    {
        [SerializeField, Min(0.05f)] private float _interval = 1f;
        [SerializeField, Min(0f)] private float _distance = 2f;
        [SerializeField, Min(0f)] private float _height = 3f;

        public override IMovementBehaviour Create() => new JumpMovement(_interval, _distance, _height);
    }
}
