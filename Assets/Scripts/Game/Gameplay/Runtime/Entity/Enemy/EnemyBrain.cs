using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Enemy))]
    internal class EnemyBrain : MonoBehaviour
    {
        private const float MinSqrMagnitudeForDirection = 0.0001f;
        
        private Enemy _self;
        private EnemySpaceScanner _spaceScanner;
        private IAbsorbable _mainTarget;
        
        private float _dangerRadius;
        private float _absorbRadius;

        private float _absorbableWeight;
        private float _preyWeight;
        private float _threatWeightMin;
        private float _threatWeightMax;
        private float _wanderWeight;
        
        private float _minBoost;
        private float _mainTargetHuntBias;
        private float _maxThreatRepulsionBoost;
        private float _maxPreyAttractionBoost;
        
        private float _minDelayWanderChange;
        private float _maxDelayWanderChange;

        private float _epsilon;       // стабилизатор
        private float _directionChangeRate;   // скорость смены направления

        private Vector2 _smoothedDirection = Vector2.right;
        private Vector2 _wanderDirection = Vector2.right;
        
        private float _nextWanderTime;
        
        public void Construct(ITargetable mainTarget, EnemyBrainData brainData)
        {
            _mainTarget = (IAbsorbable)mainTarget;
            
            _dangerRadius = brainData.DangerRadius;
            _absorbRadius = brainData.AbsorbRadius;
            float scannerRadius = Mathf.Max(_dangerRadius, _absorbRadius);
            _spaceScanner.Construct(scannerRadius);

            _absorbableWeight = brainData.AbsorbableWeight;
            _preyWeight =  brainData.PreyWeight;
            _threatWeightMin = brainData.ThreatWeightMin; 
            _threatWeightMax = brainData.ThreatWeightMax;
            _wanderWeight = brainData.WanderWeight;
        
            _minBoost = brainData.MinBoost;
            _mainTargetHuntBias = brainData.MainTargetHuntBias;
            _maxThreatRepulsionBoost = brainData.MaxThreatRepulsionBoost;
            _maxPreyAttractionBoost = brainData.MaxPreyAttractionBoost;
        
            _minDelayWanderChange = brainData.MinDelayWanderChange;
            _maxDelayWanderChange = brainData.MaxDelayWanderChange;

            _epsilon = brainData.Epsilon;
            _directionChangeRate = brainData.DirectionChangeRate;
        }

        private void Awake()
        {
            _self = GetComponent<Enemy>();
            _spaceScanner = GetComponentInChildren<EnemySpaceScanner>();
        }

        private void Update()
        {
            Vector2 selfPosition = _self.CurrentPosition;
            int selfSize = _self.Size;

            float panic;
            Vector2 threatForce = ComputeThreatRepulsion(selfPosition, selfSize, out panic);
            Vector2 absorbableForce = ComputeAbsorbablesAttraction(selfPosition);
            Vector2 preyForce = ComputePreyAttraction(selfPosition, selfSize);

            float threatWeight = Mathf.Lerp(_threatWeightMin, _threatWeightMax, panic * panic);

            Vector2 desired =
                threatForce * threatWeight +
                absorbableForce * _absorbableWeight +
                preyForce * _preyWeight;

            if (desired.sqrMagnitude < MinSqrMagnitudeForDirection)
                desired = ComputeWander() * _wanderWeight;

            Vector2 desiredDirection = Vector2.ClampMagnitude(desired, 1f);
            
            _smoothedDirection = Vector2.Lerp(_smoothedDirection, desiredDirection, _directionChangeRate * Time.deltaTime);
            
            if (_smoothedDirection.sqrMagnitude > MinSqrMagnitudeForDirection)
                _smoothedDirection.Normalize();

            _self.SetTargetDirection(_smoothedDirection);
        }

        private Vector2 ComputeThreatRepulsion(Vector2 selfPosition, int selfSize, out float panic)
        {
            float dangerRadiusSqr = _dangerRadius * _dangerRadius;
            Vector2 sum = Vector2.zero;
            float maxPanic = 0f;

            foreach (IAbsorber threat in _spaceScanner.Absorbers)
            {
                if (threat == null || !threat.IsActive) continue;
                if (ReferenceEquals(threat, _self)) continue;
                if (threat.Size <= selfSize) continue;

                Vector2 away = selfPosition - threat.CurrentPosition;
                float sqrAwayDistance = away.sqrMagnitude;
                if (sqrAwayDistance < MinSqrMagnitudeForDirection || sqrAwayDistance > dangerRadiusSqr) continue;

                float awayDistance = Mathf.Sqrt(sqrAwayDistance);
                float currentPanic = 1f - (awayDistance / _dangerRadius);
                
                if (currentPanic > maxPanic) 
                    maxPanic = currentPanic;

                float distanceWeight = 1f / (sqrAwayDistance + _epsilon); // квадрат нужен, чтобы близкие угрозы были НАМНОГО важнее дальних

                float sizeRatio = (threat.Size - selfSize) / (float)Mathf.Max(selfSize, 1);
                float sizeBoost = Mathf.Lerp(_minBoost, _maxThreatRepulsionBoost, Mathf.Clamp01(sizeRatio));

                sum += away.normalized * distanceWeight * sizeBoost;
            }

            panic = maxPanic;
            return sum;
        }

        private Vector2 ComputeAbsorbablesAttraction(Vector2 selfPosition)
        {
            float absorbableRadiusSqr = _absorbRadius * _absorbRadius;
            Vector2 sum = Vector2.zero;

            foreach (IAbsorbable absorbable in _spaceScanner.Absorbables)
            {
                if (absorbable == null || !absorbable.IsActive) continue;

                Vector2 toAbsorbable = absorbable.CurrentPosition - selfPosition;
                float sqrAbsorbableDistance = toAbsorbable.sqrMagnitude;
                
                if (sqrAbsorbableDistance < MinSqrMagnitudeForDirection || sqrAbsorbableDistance > absorbableRadiusSqr) continue;

                float distanceWeight = 1f / (sqrAbsorbableDistance + _epsilon);
                sum += toAbsorbable.normalized * distanceWeight;
            }

            return sum;
        }

        private Vector2 ComputePreyAttraction(Vector2 selfPosition, int selfSize)
        {
            float preyRadiusSqr = _absorbRadius * _absorbRadius;
            Vector2 sum = Vector2.zero;

            foreach (IAbsorber prey in _spaceScanner.Absorbers)
            {
                if (prey == null || !prey.IsActive) continue;
                if (ReferenceEquals(prey, _self)) continue;
                if (prey.Size >= selfSize) continue;

                Vector2 toPrey = prey.CurrentPosition - selfPosition;
                float sqrPreyDistance = toPrey.sqrMagnitude;
                if (sqrPreyDistance < MinSqrMagnitudeForDirection || sqrPreyDistance > preyRadiusSqr) continue;

                float distanceWeight = 1f / (sqrPreyDistance + _epsilon);

                float sizeRatio = (selfSize - prey.Size) / (float)Mathf.Max(selfSize, 1);
                float weightBoost = Mathf.Lerp(_minBoost, _maxPreyAttractionBoost, Mathf.Clamp01(sizeRatio));
                
                //близкая и маленькая добыча = огромный вклад
                //далёкая или почти равная = слабый вклад
                sum += toPrey.normalized * distanceWeight * weightBoost;
            }
            
            if (_mainTarget != null && _mainTarget.IsActive && selfSize > _mainTarget.Size)
            {
                Vector2 toMainTarget = _mainTarget.CurrentPosition - selfPosition;
                
                if (toMainTarget.sqrMagnitude > MinSqrMagnitudeForDirection)
                    sum += toMainTarget.normalized * _mainTargetHuntBias;
            }

            return sum;
        }

        private Vector2 ComputeWander()
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