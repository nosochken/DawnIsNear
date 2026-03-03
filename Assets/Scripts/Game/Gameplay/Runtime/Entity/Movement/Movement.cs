using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    internal abstract class Movement : MonoBehaviour
    {
        private const float FullSpeedFactor = 1f;

        private Rigidbody2D _rigidbody;
        
        private int _minSize;
        private float _maxSpeed;
        
        private float _maxSpeedFactorAtMinSize;
        private float _minSpeedFactorLimit;
        private float _sizeInfluenceOnSpeed;
        
        protected float MaxSpeed => _maxSpeed;
        protected Vector2 Position => _rigidbody.position;
        
        internal void Construct(MovementSpeedData speedData, int minSize, PlayField playField, 
            MovementDistanceData distanceData = null)
        {
            _minSize = minSize;
            _maxSpeed = speedData.MaxSpeed;
            
            _maxSpeedFactorAtMinSize = speedData.MaxSpeedFactorAtMinSize;
            _minSpeedFactorLimit = speedData.MinSpeedFactorLimit;
            _sizeInfluenceOnSpeed = speedData.SizeInfluenceOnSpeed;
            
            if (distanceData != null)
                ExtendConstructor(distanceData, playField);
        }

        protected virtual void ExtendConstructor(MovementDistanceData distanceData, PlayField playField) { }
    
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.freezeRotation = true;

            GetComponents();
        }
        
        protected virtual void GetComponents() { }

        protected float CalculateSizeFactor(int size)
        {
            int safeSize = Mathf.Max(_minSize, size);
            
            float factor = FullSpeedFactor / Mathf.Pow(safeSize, _sizeInfluenceOnSpeed);
            
            if (safeSize == _minSize)
                factor = Mathf.Min(factor, _maxSpeedFactorAtMinSize);

            return Mathf.Clamp(factor, _minSpeedFactorLimit, FullSpeedFactor);
        }

        protected Vector2 GetNextPosition(Vector2 velocity)
        {
            return Position + velocity * Time.fixedDeltaTime;
        }
        
        protected void Move(Vector2 nextPosition)
        {
            _rigidbody.MovePosition(nextPosition);
        }

        protected void Stop()
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }
}