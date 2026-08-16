using UnityEngine;

namespace ZooWorld.Animals.Movement
{
    public sealed class JumpMovement : IMovementBehaviour
    {
        private const float GroundProbePadding = 0.1f;

        private readonly float _interval;
        private readonly float _distance;
        private readonly float _height;


        private Rigidbody _body;
        private Vector3 _direction;
        private float _timer;
        private float _groundProbe = 0.6f;

        public JumpMovement(float interval, float distance, float height)
        {
            _interval = interval;
            _distance = distance;
            _height = height;
        }

        public void Init(Rigidbody body, Vector3 startDirection)
        {
            _body = body;
            _direction = startDirection.normalized;
            _timer = Random.Range(0f, _interval); // stagger phases so animals do not jump in sync

            _groundProbe = body.TryGetComponent(out Collider collider)
                ? collider.bounds.extents.y + GroundProbePadding
                : 0.6f;
        }

        public void SetDirection(Vector3 direction) => _direction = direction.normalized;

        public void FixedTick(float deltaTime)
        {
            if (!IsGrounded()) return;
            _timer -= deltaTime;
            if (_timer > 0f) return;

            _timer = _interval;
            Jump();
        }

        private bool IsGrounded() => Physics.Raycast(_body.position, Vector3.down, _groundProbe);

        private void Jump()
        {
            var g = Mathf.Abs(Physics.gravity.y);
            var vy = Mathf.Sqrt(2f * g * _height);
            var flightTime = 2f * vy / g;
            var vx = flightTime > 0f ? _distance / flightTime : 0f;

            _body.linearVelocity = _direction * vx + Vector3.up * vy;
        }
    }
}
