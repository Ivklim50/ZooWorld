using UnityEngine;

namespace ZooWorld.World
{
    public interface IWorldBounds
    {
        bool Contains(Vector3 worldPosition);
        Vector3 DirectionToCenter(Vector3 worldPosition);
        Vector3 RandomPointInside();
    }

    /// <summary>
    /// The play area is whatever the camera sees, projected onto the Y = 0 plane.
    /// Computed once at startup; nothing is hardcoded.
    /// </summary>
    public sealed class CameraWorldBounds : IWorldBounds
    {
        private const float Padding = 0.5f;
        private const float MinDirectionSqrMagnitude = 0.0001f;
        private readonly Rect _area; // rect x/y map to world X/Z
        private readonly Rect _safeArea;

        public CameraWorldBounds(Camera camera)
        {
            var min = ProjectToGround(camera, new Vector3(0f, 0f));
            var max = ProjectToGround(camera, new Vector3(1f, 1f));

            _area = Rect.MinMaxRect(
                Mathf.Min(min.x, max.x), Mathf.Min(min.z, max.z),
                Mathf.Max(min.x, max.x), Mathf.Max(min.z, max.z));

            _safeArea = Rect.MinMaxRect(
                _area.xMin + Padding, _area.yMin + Padding,
                _area.xMax - Padding, _area.yMax - Padding);
        }

        public bool Contains(Vector3 worldPosition)
            => _safeArea.Contains(new Vector2(worldPosition.x, worldPosition.z));

        public Vector3 DirectionToCenter(Vector3 worldPosition)
        {
            var center = new Vector3(_area.center.x, worldPosition.y, _area.center.y);
            var dir = center - worldPosition;
            dir.y = 0f;
            return dir.sqrMagnitude > MinDirectionSqrMagnitude ? dir.normalized : Vector3.forward;
        }

        public Vector3 RandomPointInside() => new(
            Random.Range(_safeArea.xMin, _safeArea.xMax),
            0f,
            Random.Range(_safeArea.yMin, _safeArea.yMax));

        /// <summary>Ray from a viewport corner onto the ground plane — works for a perspective camera too.</summary>
        private static Vector3 ProjectToGround(Camera camera, Vector3 viewportPoint)
        {
            var ray = camera.ViewportPointToRay(viewportPoint);
            var ground = new Plane(Vector3.up, Vector3.zero);
            return ground.Raycast(ray, out var distance) ? ray.GetPoint(distance) : Vector3.zero;
        }
    }
}
