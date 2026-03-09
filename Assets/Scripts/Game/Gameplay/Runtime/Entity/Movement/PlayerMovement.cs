using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    internal class PlayerMovement : Movement
    {
        private CapsuleCollider2D _collider;
        private MovementBoundary _boundary;
        private PlayField _playField;
        
        private float _maxDistance;
        private float _stopDistance;
        
        [Inject]
        private void Construct(PlayerConfig config, PlayField playField)
        {
            _playField = playField;
            
            Initialize(config.MovementSpeed, config.Size.MinSize);
            _maxDistance = config.DistanceData.MaxDistance;
            _stopDistance = config.DistanceData.StopDistance;
        }

        protected override void GetComponents()
        {
            _collider = GetComponent<CapsuleCollider2D>();
            _boundary = new MovementBoundary(_collider, _playField);
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