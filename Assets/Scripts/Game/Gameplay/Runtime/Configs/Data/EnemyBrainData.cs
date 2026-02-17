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

        [SerializeField, Min(0f)] private float _epsilon;
        [SerializeField, Min(0f)] private float _directionChangeRate;
        
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

        public float Epsilon => _epsilon;
        public float DirectionChangeRate => _directionChangeRate;
    }
}