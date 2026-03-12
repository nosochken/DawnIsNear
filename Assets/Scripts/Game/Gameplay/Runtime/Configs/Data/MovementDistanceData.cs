using System;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public class MovementDistanceData
    {
        [SerializeField, Min(0f)] private float _maxDistance ;
        [SerializeField, Min(0f)] private float _stopDistance;
        
        public float MaxDistance => _maxDistance;
        public float StopDistance => _stopDistance;
        
        public void Validate()
        {
            if (_maxDistance <= 0f)
                throw new ArgumentOutOfRangeException(nameof(_maxDistance));

            if (_stopDistance < 0f)
                throw new ArgumentOutOfRangeException(nameof(_stopDistance));

            if (_stopDistance > _maxDistance)
                throw new ArgumentException($"{nameof(_stopDistance)} must be less than or equal to {nameof(_maxDistance)}");
        }
    }
}