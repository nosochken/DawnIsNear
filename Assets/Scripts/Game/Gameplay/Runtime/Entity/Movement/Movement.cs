using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    internal abstract class Movement : MonoBehaviour
    {
        private const float FullSpeedFactor = 1f;

        private Rigidbody2D _rigidbody;

        private float _maxSpeed;
        private float _maxDistance;
        private float _stopDistance;

        private float _maxSpeedFactorAtMinSize;
        private float _minSpeedFactorLimit;
        private float _sizeInfluenceOnSpeed;
    
        private int _minSize;
        
        protected Rigidbody2D Rigidbody => _rigidbody;
        protected float MaxSpeed => _maxSpeed;
        protected float StopDistance => _stopDistance;
        
        public void Construct(MovementData data, int minSize, PlayField playField)
        {
            _maxSpeed = data.MaxSpeed;
            _maxDistance = data.MaxDistance;
            _stopDistance = data.StopDistance;

            _maxSpeedFactorAtMinSize = data.MaxSpeedFactorAtMinSize;
            _minSpeedFactorLimit = data.MinSpeedFactorLimit;
            _sizeInfluenceOnSpeed = data.SizeInfluenceOnSpeed;
        
            _minSize = minSize;
            
            ExtendConstructor(playField);
        }

        protected virtual void ExtendConstructor(PlayField playField) { }
    
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.freezeRotation = true;

            GetComponents();
        }
        
        protected virtual void GetComponents() { }

        protected float CalculateDistanceFactor(float distance)
        {
            return Mathf.Clamp01(distance / _maxDistance);
        }

        protected float CalculateSizeFactor(int size)
        {
            int safeSize = Mathf.Max(_minSize, size);
            
            float factor = FullSpeedFactor / Mathf.Pow(safeSize, _sizeInfluenceOnSpeed);
            
            if (safeSize == _minSize)
                factor = Mathf.Min(factor, _maxSpeedFactorAtMinSize);

            return Mathf.Clamp(factor, _minSpeedFactorLimit, FullSpeedFactor);
        }
    }
}