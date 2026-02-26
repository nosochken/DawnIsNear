using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    internal class PlayerMovement : Movement
    {
        private CapsuleCollider2D _collider;
        private MovementBoundaryClamp _boundaryClamp;

        protected override void ExtendConstructor(PlayField playField)
        {
            _boundaryClamp = new MovementBoundaryClamp(_collider, playField);
        }

        protected override void GetComponents()
        {
            _collider = GetComponent<CapsuleCollider2D>();
        }

        public void MoveTo(Vector2 targetPosition, int size)
        {
            Vector2 toTarget  =  targetPosition - Rigidbody.position;
            float distance = toTarget.magnitude;

            if (distance <= StopDistance)
            {
                Rigidbody.velocity = Vector2.zero;
                return;
            }

            float distanceFactor = CalculateDistanceFactor(distance);
            float sizeFactor = CalculateSizeFactor(size);

            float speed = MaxSpeed * distanceFactor * sizeFactor;
            
            Vector2 velocity = toTarget.normalized * speed;
            ApplyMovementWithBounds(velocity);
        }
        
        private void ApplyMovementWithBounds(Vector2 velocity)
        {
            Vector2 currentPosition = Rigidbody.position;
            _boundaryClamp.LimitVelocityWithBorder(ref velocity, currentPosition);
            
            Vector2 nextPosition = currentPosition + velocity * Time.fixedDeltaTime;
            _boundaryClamp.LimitPositionWithBorder(ref nextPosition);

            Rigidbody.MovePosition(nextPosition);
            //_rigidbody.velocity = (nextPosition - currentPosition) / Time.fixedDeltaTime; //если что-то будет не так
        }
    }
}