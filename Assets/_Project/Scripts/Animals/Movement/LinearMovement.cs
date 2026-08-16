using UnityEngine;

namespace ZooWorld.Animals.Movement
{
    /// <summary>Constant-speed movement in the current direction.</summary>
    public sealed class LinearMovement : IMovementBehaviour
    {
        private readonly float _speed;
        private Rigidbody _body;
        private Vector3 _direction;

        public LinearMovement(float speed) => _speed = speed;

        public void Init(Rigidbody body, Vector3 startDirection)
        {
            _body = body;
            _direction = startDirection.normalized;
        }

        public void SetDirection(Vector3 direction) => _direction = direction.normalized;

        public void FixedTick(float deltaTime)
        {
            // leave Y untouched so gravity and bounces keep working
            var v = _direction * _speed;
            v.y = _body.linearVelocity.y;
            _body.linearVelocity = v;
        }
    }
}
