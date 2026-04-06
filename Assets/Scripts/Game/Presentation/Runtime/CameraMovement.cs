using System;
using Game.Gameplay;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    public class CameraMovement : MonoBehaviour
    {
        private Camera _camera;
        private IBody _target;
        private PlayField _playField;
        
        [Inject]
        private void Construct(IBody target, PlayField playField)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _playField = playField ?? throw new ArgumentNullException(nameof(playField));
        }

        private void Awake()
        {
            _camera = Camera.main;
            
            if (_camera == null)
                throw new InvalidOperationException("Main Camera is not found.");
        }

        private void LateUpdate()
        {
            Move();
        }

        private void Move()
        {
            Vector3 position = transform.position;

            position.x = _target.CurrentPosition.x;
            position.y = _target.CurrentPosition.y;

            float halfHeight = _camera.orthographicSize;
            float halfWidth = halfHeight * _camera.aspect;
            
            Vector2 min = _playField.MinPoint;
            Vector2 max = _playField.MaxPoint;
            
            position.x = Mathf.Clamp(position.x, min.x + halfWidth, max.x - halfWidth);
            position.y = Mathf.Clamp(position.y, min.y + halfHeight, max.y - halfHeight);
            //position.z = _camera.transform.position.z;
            
            transform.position = position;
        }
    }
}