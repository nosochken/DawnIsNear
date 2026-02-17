using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    internal class Movement : MonoBehaviour
    {
        private const float FullSpeedFactor = 1f;
        private const float MinSqrMagnitudeForDirection = 0.0001f;
    
        private Rigidbody2D _rigidbody;

        private float _maxSpeed;
        private float _stopDistance;
        private float _maxDistance;

        private float _maxSpeedFactorAtMinSize;
        private float _minSpeedFactorLimit;
        private float _sizeInfluenceOnSpeed;
    
        private int _minSize;
        
        public void Construct(MovementData data, int minSize)
        {
            _maxSpeed = data.MaxSpeed;
            _stopDistance = data.StopDistance;
            _maxDistance = data.MaxDistance;

            _maxSpeedFactorAtMinSize = data.MaxSpeedFactorAtMinSize;
            _minSpeedFactorLimit = data.MinSpeedFactorLimit;
            _sizeInfluenceOnSpeed = data.SizeInfluenceOnSpeed;
        
            _minSize = minSize;
        }
    
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.freezeRotation = true;
        }

        public void MoveTo(Vector2 targetPosition, int size)
        {
            Vector2 toTarget  =  targetPosition - (Vector2)transform.position;
            float distance = toTarget.magnitude;

            if (distance <= _stopDistance)
            {
                _rigidbody.velocity = Vector2.zero;
                return;
            }

            float distanceFactor = CalculateDistanceFactor(distance);
            float sizeFactor = CalculateSizeFactor(size);

            float speed = _maxSpeed * distanceFactor * sizeFactor;
            _rigidbody.velocity = toTarget.normalized * speed;
        }
        
        public void MoveByDirection(Vector2 direction, int size)
        {
            if (direction.sqrMagnitude <= MinSqrMagnitudeForDirection)
            {
                _rigidbody.velocity = Vector2.zero;
                return;
            }
            
            float sizeFactor = CalculateSizeFactor(size);
            
            float speed = _maxSpeed * FullSpeedFactor  * sizeFactor;
            _rigidbody.velocity = direction.normalized * speed;
        }

        private float CalculateDistanceFactor(float distance)
        {
            return Mathf.Clamp01(distance / _maxDistance);
        }

        private float CalculateSizeFactor(int size)
        {
            int safeSize = Mathf.Max(_minSize, size);
            
            float factor = FullSpeedFactor / Mathf.Pow(safeSize, _sizeInfluenceOnSpeed);
            
            if (safeSize == _minSize)
                factor = Mathf.Min(factor, _maxSpeedFactorAtMinSize);

            return Mathf.Clamp(factor, _minSpeedFactorLimit, FullSpeedFactor);
        }
    }
}