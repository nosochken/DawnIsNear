using UnityEngine;

namespace Game.Gameplay
{
    internal sealed class WanderSteering
    {
        private float _minDelayWanderChange;
        private float _maxDelayWanderChange;
        
        private float _nextWanderTime;
        private Vector2 _wanderDirection = Vector2.right;
        
        internal WanderSteering(float minDelayWanderChange, float maxDelayWanderChange)
        {
            _minDelayWanderChange = minDelayWanderChange;
            _maxDelayWanderChange = maxDelayWanderChange;
        }
        
        internal Vector2 ComputeWander()
        {
            if (Time.time >= _nextWanderTime)
            {
                _nextWanderTime = Time.time + Random.Range(_minDelayWanderChange, _maxDelayWanderChange);
                _wanderDirection = Random.insideUnitCircle.normalized;
            }
            return _wanderDirection;
        }
    }
}