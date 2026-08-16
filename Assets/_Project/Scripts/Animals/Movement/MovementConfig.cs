using UnityEngine;

namespace ZooWorld.Animals.Movement
{
    /// <summary>
    /// Movement behaviour. Each individual owns its state (timers, direction).
    /// </summary>
    public interface IMovementBehaviour
    {
        void Init(Rigidbody body, Vector3 startDirection);

        /// <summary>Called from FixedUpdate of a living animal.</summary>
        void FixedTick(float deltaTime);

        /// <summary>Turn at the world boundary.</summary>
        void SetDirection(Vector3 direction);
    }
    public abstract class MovementConfig : ScriptableObject
    {
        public abstract IMovementBehaviour Create();
    }
}
