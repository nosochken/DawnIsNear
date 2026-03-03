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
    }
}