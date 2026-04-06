using System;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    internal class EnemyBrain : MonoBehaviour
    {
        private IBody _mainTarget;
        private EnemySpaceScanner _spaceScanner;
        private PlayField _playField;
       
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

        [Inject]
        private void Construct(IBody mainTarget, PlayField playField)
        {
            _mainTarget = mainTarget ?? throw new ArgumentNullException(nameof(mainTarget));
            _playField = playField ?? throw new ArgumentNullException(nameof(playField));;
        }

        internal void Initialize(Transform selfTransform, CapsuleCollider2D collider, EnemyBrainData data)
        {
            if (selfTransform == null)
                throw new ArgumentNullException(nameof(selfTransform));
            
            if (collider == null)
                throw new ArgumentNullException(nameof(collider));
            
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            
            float scannerRadius = Mathf.Max(data.DangerRadius, data.AbsorbRadius);
            
            _spaceScanner.Initialize(selfTransform, scannerRadius);
            
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
            
            _boundarySteering = new BoundarySteering(collider, _playField, data.BoundaryAvoidDistance,
                data.BoundaryDistanceEpsilon, data.BoundaryThreshold);
            
            _wanderSteering = new WanderSteering(data.MinDelayWanderChange, data.MaxDelayWanderChange);
        }

        private void Awake()
        {
            _spaceScanner = GetComponentInChildren<EnemySpaceScanner>();
            
            if (_spaceScanner == null)
                throw new InvalidOperationException("EnemySpaceScanner is not found.");
        }

        internal Vector2 GetBestTarget(Vector2 position, int size)
        {
            Vector2 selfPosition = position;
            int selfSize = size;
            
            Vector2 desiredDirection = ComputeDesiredDirection(out Vector2 threatForce, selfPosition, selfSize);
            desiredDirection = _boundarySteering.ResolveDirection(desiredDirection, selfPosition, threatForce);
            
            return GetSmoothedDirection(desiredDirection);
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

            if (desired.sqrMagnitude < DirectionMath.MinSqrMagnitudeForDirection)
                desired = _wanderSteering.ComputeWander() * _wanderWeight;
            
            return NormalizeDirectionSafe(desired);
        }

        private Vector2 GetSmoothedDirection(Vector2 desiredDirection)
        {
            _smoothedDirection = Vector2.Lerp(_smoothedDirection, desiredDirection, _directionChangeRate * Time.deltaTime); 
            return NormalizeDirectionSafe(_smoothedDirection);
        }
        
        private Vector2 NormalizeDirectionSafe(Vector2 vector)
        {
            float sqrMagnitude = vector.sqrMagnitude;

            if (sqrMagnitude < DirectionMath.MinSqrMagnitudeForDirection)
                return Vector2.right;
            
            if (sqrMagnitude > 1f)
                return vector.normalized;
            
            return vector;
        }
    }
}