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
    }
}