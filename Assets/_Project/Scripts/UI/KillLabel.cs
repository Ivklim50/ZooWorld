using UnityEngine;
using TMPro;

namespace ZooWorld.UI
{
    public sealed class KillLabel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField, Min(0.1f)] private float _lifetime = 1f;

        private Camera _camera;
        private Vector3 _offset;

        public void Play(string message, Camera targetCamera, Vector3 offset)
        {
            _text.text = message;
            _camera = targetCamera;
            _offset = offset;

            UpdatePlacement();
            Destroy(gameObject, _lifetime);
        }

        private void LateUpdate() => UpdatePlacement();

        private void UpdatePlacement()
        {
            var owner = transform.parent;
            if (owner != null) transform.position = owner.position + _offset;

            if (_camera != null) transform.rotation = _camera.transform.rotation;
        }
    }
}
