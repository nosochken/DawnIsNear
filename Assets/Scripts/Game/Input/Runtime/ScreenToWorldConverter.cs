using System;
using UnityEngine;

namespace Game.Input
{
    public class ScreenToWorldConverter : MonoBehaviour
    {
        private Camera _camera;
        
        private void Awake()
        {
            _camera = Camera.main;

            if (_camera == null)
                throw new InvalidOperationException("Main Camera is not found.");
        }

        public Vector2 Convert(Vector3 screenPosition)
        {
            screenPosition.z = -_camera.transform.position.z;
            return _camera.ScreenToWorldPoint(screenPosition);
        }
    }
}