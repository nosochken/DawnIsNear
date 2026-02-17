using UnityEngine;

namespace Game.Presentation
{
    public class CameraMovement : MonoBehaviour
    {
        private Camera _camera;
        private ITargetable _target;
        
        public void Construct(ITargetable  target)
        {
            _target = target;
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
            Vector3 targetPosition = new Vector3(_target.CurrentPosition.x, 
                _target.CurrentPosition.y, _camera.transform.position.z);

            transform.position = targetPosition;
        }
    }
}