using UnityEngine;
using VContainer;
using ZooWorld.Animals;
using ZooWorld.Stats;

namespace ZooWorld.UI
{
    public sealed class KillLabelSpawner : MonoBehaviour
    {
        [SerializeField] private KillLabel _labelPrefab;
        [SerializeField] private Vector3 _offset = new(0f, 0.5f, -1f);

        private IGameEvents _events;
        private Camera _camera;

        [Inject]
        public void Construct(IGameEvents events, Camera camera)
        {
            _events = events;
            _camera = camera;
            _events.Killed += OnKilled;
        }

        private void OnDestroy()
        {
            if (_events != null) _events.Killed -= OnKilled;
        }

        private void OnKilled(Animal killer, Animal victim)
        {
            if (killer == null) return;

            var message = killer.Definition.KillLabel;
            if (string.IsNullOrEmpty(message)) return;   

            var label = Instantiate(_labelPrefab, killer.transform);
            label.Play(message, _camera, _offset);
        }
    }
}
