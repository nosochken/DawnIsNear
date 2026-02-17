using UnityEngine;

namespace Game.Input
{
    public class ScreenToWorld : MonoBehaviour
    {
        private Camera _camera;
        
        private void Awake()
        {
            _camera = Camera.main;
        }

        public Vector2 Convert(Vector3 screenPosition)
        {
            screenPosition.z = -_camera.transform.position.z;
            return _camera.ScreenToWorldPoint(screenPosition);
        }
    }
}