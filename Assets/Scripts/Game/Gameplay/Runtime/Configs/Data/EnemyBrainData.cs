using System;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public class EnemyBrainData
    {
        [SerializeField, Min(0f)] private float _dangerRadius;
        [SerializeField, Min(0f)] private float _absorbRadius;

        [SerializeField, Min(0f)] private float _absorbableWeight;
        [SerializeField, Min(0f)] private float _preyWeight;
        [SerializeField, Min(0f)] private float _threatWeightMin;
        [SerializeField, Min(0f)] private float _threatWeightMax;
        [SerializeField, Min(0f)] private float _wanderWeight;
        
        [SerializeField, Min(0f)] private float _minBoost;
        [SerializeField, Min(0f)] private float _mainTargetHuntBias;
        [SerializeField, Min(0f)] private float _maxThreatRepulsionBoost;
        [SerializeField, Min(0f)] private float _maxPreyAttractionBoost;
        
        [SerializeField, Min(0f)] private float _minDelayWanderChange;
        [SerializeField, Min(0f)] private float _maxDelayWanderChange;

        [SerializeField, Min(0f)] private float _sqrDistanceEpsilon;
        [SerializeField, Min(0f)] private float _directionChangeRate;
        
        [SerializeField, Min(0f)] private float _boundaryWeight;
        [SerializeField, Min(0f)] private float _boundaryAvoidDistance;
        [SerializeField, Min(0f)] private float _boundaryThreshold;
        [SerializeField, Min(0f)] private float _boundaryDistanceEpsilon;
        
        public float DangerRadius => _dangerRadius;
        public float AbsorbRadius => _absorbRadius;

        public float AbsorbableWeight => _absorbableWeight;
        public float PreyWeight => _preyWeight;
        public float ThreatWeightMin => _threatWeightMin;
        public float ThreatWeightMax => _threatWeightMax;
        public float WanderWeight => _wanderWeight;
        
        public float MinBoost => _minBoost;
        public float MainTargetHuntBias => _mainTargetHuntBias;
        public float MaxThreatRepulsionBoost => _maxThreatRepulsionBoost;
        public float MaxPreyAttractionBoost => _maxPreyAttractionBoost;
        
        public float MinDelayWanderChange => _minDelayWanderChange;
        public float MaxDelayWanderChange => _maxDelayWanderChange;

        public float SqrDistanceEpsilon => _sqrDistanceEpsilon;
        public float DirectionChangeRate => _directionChangeRate;
        
        public float BoundaryWeight => _boundaryWeight;
        public float BoundaryAvoidDistance => _boundaryAvoidDistance;
        public float BoundaryThreshold => _boundaryThreshold;
        public float BoundaryDistanceEpsilon => _boundaryDistanceEpsilon;
        
        public void Validate()
        {
            if (_dangerRadius <= 0f)
                throw new ArgumentOutOfRangeException(nameof(_dangerRadius));

            if (_absorbRadius <= 0f)
                throw new ArgumentOutOfRangeException(nameof(_absorbRadius));

            if (_absorbableWeight < 0f)
                throw new ArgumentOutOfRangeException(nameof(_absorbableWeight));

            if (_preyWeight < 0f)
                throw new ArgumentOutOfRangeException(nameof(_preyWeight));

            if (_threatWeightMin < 0f)
                throw new ArgumentOutOfRangeException(nameof(_threatWeightMin));

            if (_threatWeightMax < _threatWeightMin)
                throw new ArgumentException(
                    $"{nameof(_threatWeightMax)} must be greater than or equal to {nameof(_threatWeightMin)}");

            if (_wanderWeight < 0f)
                throw new ArgumentOutOfRangeException(nameof(_wanderWeight));

            if (_minBoost < 0f)
                throw new ArgumentOutOfRangeException(nameof(_minBoost));

            if (_mainTargetHuntBias < 0f)
                throw new ArgumentOutOfRangeException(nameof(_mainTargetHuntBias));

            if (_maxThreatRepulsionBoost < _minBoost)
                throw new ArgumentException($"{nameof(_maxThreatRepulsionBoost)} must be greater than or equal to {nameof(_minBoost)}");

            if (_maxPreyAttractionBoost < _minBoost)
                throw new ArgumentException($"{nameof(_maxPreyAttractionBoost)} must be greater than or equal to {nameof(_minBoost)}");

            if (_minDelayWanderChange < 0f)
                throw new ArgumentOutOfRangeException(nameof(_minDelayWanderChange));

            if (_maxDelayWanderChange < _minDelayWanderChange)
                throw new ArgumentException($"{nameof(_maxDelayWanderChange)} must be greater than or equal to {nameof(_minDelayWanderChange)}");

            if (_sqrDistanceEpsilon <= 0f)
                throw new ArgumentOutOfRangeException(nameof(_sqrDistanceEpsilon));

            if (_directionChangeRate < 0f)
                throw new ArgumentOutOfRangeException(nameof(_directionChangeRate));

            if (_boundaryWeight < 0f)
                throw new ArgumentOutOfRangeException(nameof(_boundaryWeight));

            if (_boundaryAvoidDistance <= 0f)
                throw new ArgumentOutOfRangeException(nameof(_boundaryAvoidDistance));

            if (_boundaryThreshold < 0f)
                throw new ArgumentOutOfRangeException(nameof(_boundaryThreshold));

            if (_boundaryDistanceEpsilon <= 0f)
                throw new ArgumentOutOfRangeException(nameof(_boundaryDistanceEpsilon));
        }
    }
}