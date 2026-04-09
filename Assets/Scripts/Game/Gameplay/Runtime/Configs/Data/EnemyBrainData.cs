using System;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public class EnemyBrainData
    {
        [SerializeField, Min(0f)] private float _dangerRadius;
        [SerializeField, Min(0f)] private float _absorbRadius;
        
        [SerializeField, Min(0f)] private float _minAbsorberRepulsionWeight;
        [SerializeField, Min(0f)] private float _maxAbsorberRepulsionWeight;
        [SerializeField, Min(0f)] private float _absorbableAttractionWeight;
        [SerializeField, Min(0f)] private float _wanderWeight;
        
        [SerializeField, Min(0f)] private float _minBoost;
        [SerializeField, Min(0f)] private float _mainTargetAttractionBias;
        [SerializeField, Min(0f)] private float _maxAbsorberRepulsionBoost;
        [SerializeField, Min(0f)] private float _maxAbsorbableAttractionBoost;
        
        [SerializeField, Min(0f)] private float _minDelayWanderChange;
        [SerializeField, Min(0f)] private float _maxDelayWanderChange;

        [SerializeField, Min(0f)] private float _sqrDistanceEpsilon;
        [SerializeField, Min(0f)] private float _directionChangeRate;
        
        [SerializeField, Min(0f)] private float _boundaryRepulsionWeight;
        [SerializeField, Min(0f)] private float _boundaryAvoidDistance;
        [SerializeField, Min(0f)] private float _boundaryThreshold;
        [SerializeField, Min(0f)] private float _boundaryDistanceEpsilon;
        
        public float DangerRadius => _dangerRadius;
        public float AbsorbRadius => _absorbRadius;

        public float MinAbsorberRepulsionWeight => _minAbsorberRepulsionWeight;
        public float MaxAbsorberRepulsionWeight => _maxAbsorberRepulsionWeight;
        public float AbsorbableAttractionWeight => _absorbableAttractionWeight;
        public float WanderWeight => _wanderWeight;
        
        public float MinBoost => _minBoost;
        public float MainTargetAttractionBias => _mainTargetAttractionBias;
        public float MaxAbsorberRepulsionBoost => _maxAbsorberRepulsionBoost;
        public float MaxAbsorbableAttractionBoost => _maxAbsorbableAttractionBoost;
        
        public float MinDelayWanderChange => _minDelayWanderChange;
        public float MaxDelayWanderChange => _maxDelayWanderChange;

        public float SqrDistanceEpsilon => _sqrDistanceEpsilon;
        public float DirectionChangeRate => _directionChangeRate;
        
        public float BoundaryRepulsionWeight => _boundaryRepulsionWeight;
        public float BoundaryAvoidDistance => _boundaryAvoidDistance;
        public float BoundaryThreshold => _boundaryThreshold;
        public float BoundaryDistanceEpsilon => _boundaryDistanceEpsilon;
        
        public void Validate()
        {
            if (_dangerRadius <= 0f)
                throw new ArgumentOutOfRangeException(nameof(_dangerRadius));

            if (_absorbRadius <= 0f)
                throw new ArgumentOutOfRangeException(nameof(_absorbRadius));

            if (_minAbsorberRepulsionWeight < 0f)
                throw new ArgumentOutOfRangeException(nameof(_minAbsorberRepulsionWeight));

            if (_maxAbsorberRepulsionWeight < _minAbsorberRepulsionWeight)
                throw new ArgumentException(
                    $"{nameof(_maxAbsorberRepulsionWeight)} must be greater than or equal to {nameof(_minAbsorberRepulsionWeight)}");
            
            if (_absorbableAttractionWeight < 0f)
                throw new ArgumentOutOfRangeException(nameof(_absorbableAttractionWeight));

            if (_wanderWeight < 0f)
                throw new ArgumentOutOfRangeException(nameof(_wanderWeight));

            if (_minBoost < 0f)
                throw new ArgumentOutOfRangeException(nameof(_minBoost));

            if (_mainTargetAttractionBias < 0f)
                throw new ArgumentOutOfRangeException(nameof(_mainTargetAttractionBias));

            if (_maxAbsorberRepulsionBoost < _minBoost)
                throw new ArgumentException($"{nameof(_maxAbsorberRepulsionBoost)} must be greater than or equal to {nameof(_minBoost)}");

            if (_maxAbsorbableAttractionBoost < _minBoost)
                throw new ArgumentException($"{nameof(_maxAbsorbableAttractionBoost)} must be greater than or equal to {nameof(_minBoost)}");

            if (_minDelayWanderChange < 0f)
                throw new ArgumentOutOfRangeException(nameof(_minDelayWanderChange));

            if (_maxDelayWanderChange < _minDelayWanderChange)
                throw new ArgumentException($"{nameof(_maxDelayWanderChange)} must be greater than or equal to {nameof(_minDelayWanderChange)}");

            if (_sqrDistanceEpsilon <= 0f)
                throw new ArgumentOutOfRangeException(nameof(_sqrDistanceEpsilon));

            if (_directionChangeRate < 0f)
                throw new ArgumentOutOfRangeException(nameof(_directionChangeRate));

            if (_boundaryRepulsionWeight < 0f)
                throw new ArgumentOutOfRangeException(nameof(_boundaryRepulsionWeight));

            if (_boundaryAvoidDistance <= 0f)
                throw new ArgumentOutOfRangeException(nameof(_boundaryAvoidDistance));

            if (_boundaryThreshold < 0f)
                throw new ArgumentOutOfRangeException(nameof(_boundaryThreshold));

            if (_boundaryDistanceEpsilon <= 0f)
                throw new ArgumentOutOfRangeException(nameof(_boundaryDistanceEpsilon));
        }
    }
}