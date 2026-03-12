using System;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public class MovementSpeedData
    {
        [SerializeField, Min(0f)] private float _maxSpeed;
        [SerializeField, Range(0f, 1f)] private float _maxSpeedFactorAtMinSize;
        [SerializeField, Min(0f)] private float _minSpeedFactorLimit;
        [SerializeField, Min(0f)] private float _sizeInfluenceOnSpeed;
        
        public float MaxSpeed => _maxSpeed;
        public float MaxSpeedFactorAtMinSize => _maxSpeedFactorAtMinSize;
        public float MinSpeedFactorLimit => _minSpeedFactorLimit;
        public float SizeInfluenceOnSpeed => _sizeInfluenceOnSpeed;
        
        public void Validate()
        {
            if (_maxSpeed <= 0f)
                throw new ArgumentOutOfRangeException(nameof(_maxSpeed));

            if (_minSpeedFactorLimit < 0f || _minSpeedFactorLimit > 1f)
                throw new ArgumentOutOfRangeException(nameof(_minSpeedFactorLimit));

            if (_maxSpeedFactorAtMinSize < _minSpeedFactorLimit) 
                throw new ArgumentException($"{nameof(_maxSpeedFactorAtMinSize)} must be greater than or equal to {nameof(_minSpeedFactorLimit)}");

            if (_sizeInfluenceOnSpeed < 0f)
                throw new ArgumentOutOfRangeException(nameof(_sizeInfluenceOnSpeed));
        }
    }
}