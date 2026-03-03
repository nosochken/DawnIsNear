using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    internal class PlayerMovement : Movement
    {
        private CapsuleCollider2D _collider;
        private MovementBoundary _boundary;
        
        private float _maxDistance;
        private float _stopDistance;

        protected override void ExtendConstructor(MovementDistanceData distanceData, PlayField playField)
        {
            _maxDistance = distanceData.MaxDistance;
            _stopDistance = distanceData.StopDistance;
            
            _boundary = new MovementBoundary(_collider, playField);
        }

        protected override void GetComponents()
        {
            _collider = GetComponent<CapsuleCollider2D>();
        }

        internal void MoveTo(Vector2 targetPosition, int size)
        {
            Vector2 toTarget  =  targetPosition - Position;
            float distance = toTarget.magnitude;

            if (distance <= _stopDistance)
            {
                Stop();
                return;
            }

            float distanceFactor = CalculateDistanceFactor(distance);
            float sizeFactor = CalculateSizeFactor(size);

            float speed = MaxSpeed * distanceFactor * sizeFactor;
            
            Vector2 velocity = toTarget.normalized * speed;
            ApplyMovementWithBoundaries(velocity);
        }
        
        private float CalculateDistanceFactor(float distance)
        {
            return Mathf.Clamp01(distance / _maxDistance);
        }
        
        private void ApplyMovementWithBoundaries(Vector2 velocity)
        {
            Vector2 currentPosition = Position;
            _boundary.BlockOutboundAxes(ref velocity, currentPosition);
            
            Vector2 nextPosition = GetNextPosition(velocity);
            _boundary.ClampPosition(ref nextPosition);
            
            Move(nextPosition);
        }
    }
}