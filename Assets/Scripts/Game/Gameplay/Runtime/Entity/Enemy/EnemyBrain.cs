using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Enemy))]
    internal class EnemyBrain : MonoBehaviour
    {
        private Enemy _self;
        private IAbsorbable _mainTarget;
        private EnemySpaceScanner _spaceScanner;
       
        private ThreatSteering _threatSteering;
        private AbsorbableSteering _absorbableSteering;
        private PreySteering _preySteering;
        private BoundarySteering _boundarySteering;
        private WanderSteering _wanderSteering;

        private float _absorbableWeight;
        private float _preyWeight;
        private float _threatWeightMin;
        private float _threatWeightMax;
        private float _wanderWeight;
        private float _boundaryWeight;
        
        private float _directionChangeRate;
        private Vector2 _smoothedDirection = Vector2.right;
        
        public void Construct(CapsuleCollider2D collider, EnemyBrainData data, ITargetable mainTarget, PlayField playField)
        {
            _mainTarget = (IAbsorbable)mainTarget;
            
            float scannerRadius = Mathf.Max(data.DangerRadius, data.AbsorbRadius);
            _spaceScanner.Construct(_self.transform, scannerRadius);

            _absorbableWeight = data.AbsorbableWeight;
            _preyWeight =  data.PreyWeight;
            _threatWeightMin = data.ThreatWeightMin; 
            _threatWeightMax = data.ThreatWeightMax;
            _wanderWeight = data.WanderWeight;
            _boundaryWeight = data.BoundaryWeight;
            
            _directionChangeRate = data.DirectionChangeRate;

            _threatSteering = new ThreatSteering(data.DangerRadius, data.MinBoost,
                data.MaxThreatRepulsionBoost, data.SqrDistanceEpsilon);
            
            _absorbableSteering = new AbsorbableSteering(data.AbsorbRadius, data.SqrDistanceEpsilon);
            
            _preySteering = new PreySteering(data.AbsorbRadius, data.MinBoost,
                data.MaxPreyAttractionBoost, data.MainTargetHuntBias, data.SqrDistanceEpsilon);
            
            _boundarySteering = new BoundarySteering(collider, playField, data.BoundaryAvoidDistance,
                data.BoundaryDistanceEpsilon, data.BoundaryThreshold);
            
            _wanderSteering = new WanderSteering(data.MinDelayWanderChange, data.MaxDelayWanderChange);
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
            
            Vector2 desiredDirection = ComputeDesiredDirection(out Vector2 threatForce, selfPosition, selfSize);
            desiredDirection = _boundarySteering.ResolveDirection(desiredDirection, selfPosition, threatForce);
            
            ApplySmoothedDirection(desiredDirection);
        }

        private Vector2 ComputeDesiredDirection(out Vector2 threatForce, Vector2 selfPosition, int selfSize)
        {
            threatForce = _threatSteering.ComputeThreatRepulsion(out float panic, _spaceScanner.Absorbers, selfPosition, selfSize);
            Vector2 absorbableForce = _absorbableSteering.ComputeAbsorbablesAttraction(_spaceScanner.Absorbables, selfPosition);
            Vector2 preyForce = _preySteering.ComputePreyAttraction(_spaceScanner.Absorbers, _mainTarget, selfPosition, selfSize);
            Vector2 boundaryForce = _boundarySteering.ComputeBoundaryRepulsion(selfPosition);
            
            float threatWeight = Mathf.Lerp(_threatWeightMin, _threatWeightMax, panic * panic);
            
            Vector2 desired =
                threatForce * threatWeight +
                absorbableForce * _absorbableWeight +
                preyForce * _preyWeight +
                boundaryForce * _boundaryWeight;

            if (desired.sqrMagnitude < SteeringMath.MinSqrMagnitudeForDirection)
                desired = _wanderSteering.ComputeWander() * _wanderWeight;
            
            return NormalizeDirectionSafe(desired);
        }

        private void ApplySmoothedDirection(Vector2 desiredDirection)
        {
            _smoothedDirection = Vector2.Lerp(_smoothedDirection, desiredDirection, _directionChangeRate * Time.deltaTime); 
            _smoothedDirection = NormalizeDirectionSafe(_smoothedDirection);

            _self.SetTargetDirection(_smoothedDirection);
        }
        
        private Vector2 NormalizeDirectionSafe(Vector2 vector)
        {
            float sqrMagnitude = vector.sqrMagnitude;

            if (sqrMagnitude < SteeringMath.MinSqrMagnitudeForDirection)
                return Vector2.right;
            
            if (sqrMagnitude > 1f)
                return vector.normalized;
            
            return vector;
        }
    }
}