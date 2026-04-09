using System;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    internal class EnemyBrain : MonoBehaviour
    {
        private IAbsorbable _mainTarget;
        private EnemySpaceScanner _spaceScanner;
        private PlayField _playField;
       
        private AbsorberRepulsionSteering _absorberRepulsionSteering;
        private AbsorbableAttractionSteering _absorbableAttractionSteering;
        private BoundaryRepulsionSteering _boundaryRepulsionSteering;
        private WanderSteering _wanderSteering;

        private float _minAbsorberRepulsionWeight;
        private float _maxAbsorberRepulsionWeight;
        private float _absorbableAttractionWeight;
        private float _boundaryRepulsionWeight;
        private float _wanderWeight;
        
        private float _directionChangeRate;
        private Vector2 _smoothedDirection = Vector2.right;

        [Inject]
        private void Construct(IAbsorbable mainTarget, PlayField playField)
        {
            _mainTarget = mainTarget ?? throw new ArgumentNullException(nameof(mainTarget));
            _playField = playField ?? throw new ArgumentNullException(nameof(playField));
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
            
            _minAbsorberRepulsionWeight = data.MinAbsorberRepulsionWeight; 
            _maxAbsorberRepulsionWeight = data.MaxAbsorberRepulsionWeight;
            _absorbableAttractionWeight = data.AbsorbableAttractionWeight;
            _boundaryRepulsionWeight = data.BoundaryRepulsionWeight;
            _wanderWeight = data.WanderWeight;
            
            _directionChangeRate = data.DirectionChangeRate;

            _absorberRepulsionSteering = new AbsorberRepulsionSteering(data.DangerRadius, data.MinBoost,
                data.MaxAbsorberRepulsionBoost, data.SqrDistanceEpsilon);
            
            _absorbableAttractionSteering = new AbsorbableAttractionSteering(data.AbsorbRadius, data.MinBoost, 
                data.MaxAbsorbableAttractionBoost, data.MainTargetAttractionBias, data.SqrDistanceEpsilon);
            
            _boundaryRepulsionSteering = new BoundaryRepulsionSteering(collider, _playField, data.BoundaryAvoidDistance,
                data.BoundaryDistanceEpsilon, data.BoundaryThreshold);
            
            _wanderSteering = new WanderSteering(data.MinDelayWanderChange, data.MaxDelayWanderChange);
        }

        private void Awake()
        {
            _spaceScanner = GetComponentInChildren<EnemySpaceScanner>();
            
            if (_spaceScanner == null)
                throw new InvalidOperationException("EnemySpaceScanner is not found.");
        }

        internal Vector2 GetBestTarget(Enemy self)
        {
            Vector2 desiredDirection = ComputeDesiredDirection(out Vector2 absorberRepulsionForce, self);
            desiredDirection = _boundaryRepulsionSteering.ResolveDirection(desiredDirection, self.Body.CurrentPosition, absorberRepulsionForce);
            
            return GetSmoothedDirection(desiredDirection);
        }

        private Vector2 ComputeDesiredDirection(out Vector2 absorberRepulsionForce, Enemy self)
        {
            absorberRepulsionForce = _absorberRepulsionSteering.Compute(out float panic, _spaceScanner.Absorbers, self.Absorbable);
            Vector2 absorbableAttractionForce = _absorbableAttractionSteering.Compute(_spaceScanner.Absorbables, _mainTarget, self.Absorber);
            Vector2 boundaryRepulsionForce = _boundaryRepulsionSteering.Compute(self.Body.CurrentPosition);
            
            float absorberRepulsionWeight = Mathf.Lerp(_minAbsorberRepulsionWeight, _maxAbsorberRepulsionWeight, panic * panic);
            
            Vector2 desired =
                absorberRepulsionForce * absorberRepulsionWeight +
                absorbableAttractionForce * _absorbableAttractionWeight +
                boundaryRepulsionForce * _boundaryRepulsionWeight;

            if (desired.sqrMagnitude < DirectionMath.MinSqrMagnitudeForDirection)
                desired = _wanderSteering.Compute() * _wanderWeight;
            
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