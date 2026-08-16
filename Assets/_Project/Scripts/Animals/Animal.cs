using UnityEngine;
using ZooWorld.Animals.Movement;
using ZooWorld.Stats;
using ZooWorld.World;

namespace ZooWorld.Animals
{
    /// <summary>
    /// An animal is not a type but a composition: species data + movement strategy + shared rules.
    /// 
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public sealed class Animal : MonoBehaviour
    {
        [SerializeField, Min(0f)]
        [Tooltip("Window after a collision when physics alone drives movement — otherwise the knockback is cancelled on the same frame")]
        private float _bounceLockDuration = 0.4f;

        private IMovementBehaviour _movement;
        private IWorldBounds _bounds;
        private IFoodChainResolver _foodChain;
        private IGameEvents _events;

        private Rigidbody _body;
        private float _bounceLockUntil;

        public AnimalDefinition Definition { get; private set; }
        public bool IsAlive { get; private set; }

        /// <summary>Called by AnimalFactory right after instantiation.</summary>
        public void Setup(
            AnimalDefinition definition,
            IWorldBounds bounds,
            IFoodChainResolver foodChain,
            IGameEvents events,
            Vector3 startDirection)
        {
            Definition = definition;
            _bounds = bounds;
            _foodChain = foodChain;
            _events = events;

            _body = GetComponent<Rigidbody>();
            _body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            _movement = definition.Movement.Create();
            _movement.Init(_body, startDirection);

            IsAlive = true;
        }

        private void FixedUpdate()
        {
            if (!IsAlive) return;

            // Out of bounds — turn back toward the center.
            if (!_bounds.Contains(transform.position))
            {
                var toCenter = _bounds.DirectionToCenter(transform.position);
                _movement.SetDirection(toCenter);

                // Flip the current horizontal velocity as well: an airborne animal would
                // otherwise keep flying outward until it lands.
                var velocity = _body.linearVelocity;
                var horizontal = new Vector3(velocity.x, 0f, velocity.z);

                if (Vector3.Dot(horizontal, toCenter) < 0f)
                    _body.linearVelocity = toCenter * horizontal.magnitude + Vector3.up * velocity.y;
            }

            // During the bounce window do not override the impulse received from physics.
            if (Time.time < _bounceLockUntil) return;

            _movement.FixedTick(Time.fixedDeltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!IsAlive) return;
            if (!collision.collider.TryGetComponent(out Animal other) || !other.IsAlive) return;

            // The collision is reported to both participants. Handle each pair exactly once.
            if (GetInstanceID() > other.GetInstanceID()) return;

            ResolveWith(other);
        }

        private void ResolveWith(Animal other)
        {
            var result = _foodChain.Resolve(Definition.Diet, other.Definition.Diet);

            switch (result)
            {
                case InteractionResult.AEatsB:
                    Kill(other);
                    break;

                case InteractionResult.BEatsA:
                    other.Kill(this);
                    break;

                case InteractionResult.Mutual:
                    // Both can eat each other (predator + predator).
                    // Deterministic tie-break: the smaller InstanceID survives.
                    Kill(other);
                    break;

                case InteractionResult.Bounce:
                    LockForBounce();
                    other.LockForBounce();
                    break;
            }
        }

        private void Kill(Animal victim)
        {
            victim.Die();
            _events.RaiseKilled(this, victim);
        }

        private void LockForBounce() => _bounceLockUntil = Time.time + _bounceLockDuration;

        public void Die()
        {
            if (!IsAlive) return;
            IsAlive = false;

            _events.RaiseDied(this);

            // TODO: return to a pool instead of Destroy once the project grows.
            Destroy(gameObject);
        }
    }
}
