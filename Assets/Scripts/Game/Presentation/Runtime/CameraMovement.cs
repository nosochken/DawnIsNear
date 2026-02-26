using Game.Gameplay;
using UnityEngine;

namespace Game.Presentation
{
    public class CameraMovement : MonoBehaviour
    {
        private Camera _camera;
        private ITargetable _target;
        private PlayField _playField;
        
        public void Construct(ITargetable  target, PlayField playField)
        {
            _target = target;
            _playField = playField;
        }

        private void Awake()
        {
            _camera = Camera.main;
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