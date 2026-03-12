using System;
using UnityEngine;

namespace Game.Gameplay
{
    internal sealed class BoundarySteering
    {
        private const float AxisComponentEpsilon = 0.0001f;

        private readonly MovementBoundary _boundary;

        private readonly float _avoidDistance;
        private readonly float _epsilon;
        private readonly float _boundaryThreshold;

        internal BoundarySteering(CapsuleCollider2D collider, PlayField playField,
            float avoidDistance, float epsilon, float boundaryThreshold)
        {
            if (collider == null)
                throw new ArgumentNullException(nameof(collider));
            
            if (playField == null)
                throw new ArgumentNullException(nameof(playField));
            
            _boundary = new MovementBoundary(collider, playField);

            _avoidDistance = avoidDistance;
            _epsilon = epsilon;
            _boundaryThreshold = boundaryThreshold;
        }

        internal Vector2 ComputeBoundaryRepulsion(Vector2 selfPosition)
        {
            _boundary.GetAllowedBounds(out float minAllowedX, out float maxAllowedX, 
                out float minAllowedY, out float maxAllowedY);

            float distanceToLeftBoundary = selfPosition.x - minAllowedX;
            float distanceToRightBoundary = maxAllowedX - selfPosition.x;
            float distanceToBottomBoundary = selfPosition.y - minAllowedY;
            float distanceToTopBoundary = maxAllowedY - selfPosition.y;

            Vector2 force = Vector2.zero;

            force += ComputeRepulsionFromBoundary(Vector2.right, distanceToLeftBoundary);
            force += ComputeRepulsionFromBoundary(Vector2.left, distanceToRightBoundary);
            force += ComputeRepulsionFromBoundary(Vector2.up, distanceToBottomBoundary);
            force += ComputeRepulsionFromBoundary(Vector2.down, distanceToTopBoundary);

            return force;
        }

        internal Vector2 ResolveDirection(Vector2 desiredDirection, Vector2 selfPosition, Vector2 threatForce)
        {
            Vector2 adjustedDirection = desiredDirection;
            _boundary.BlockOutboundAxes(ref adjustedDirection, selfPosition, _boundaryThreshold);

            if (adjustedDirection.sqrMagnitude > DirectionMath.MinSqrMagnitudeForDirection)
                return adjustedDirection.normalized;
            
            _boundary.GetNearFlags(out bool nearMinX, out bool nearMaxX, out bool nearMinY, out bool nearMaxY, 
                selfPosition, _boundaryThreshold );
            
            bool isNearVerticalBoundary = nearMinX || nearMaxX;
            bool isNearHorizontalBoundary = nearMinY || nearMaxY;

            Vector2 fallbackDirection = SelectFallbackDirection(threatForce, desiredDirection);

            return SelectAxisSlideDirection(isNearVerticalBoundary, isNearHorizontalBoundary, fallbackDirection,
                desiredDirection);
        }
        
        private Vector2 ComputeRepulsionFromBoundary(Vector2 awayDirection, float distance)
        {
            float safeDistance = Mathf.Max(distance, 0f);

            if (safeDistance >= _avoidDistance)
                return Vector2.zero;

            float closeness = 1f - Mathf.Clamp01(safeDistance / _avoidDistance);
            float weight = (closeness * closeness) / (safeDistance + _epsilon);

            return awayDirection * weight;
        }
        
        private Vector2 SelectFallbackDirection(Vector2 threatForce, Vector2 desiredDirection)
        {
            if (threatForce.sqrMagnitude > DirectionMath.MinSqrMagnitudeForDirection)
                return threatForce.normalized;

            if (desiredDirection.sqrMagnitude > DirectionMath.MinSqrMagnitudeForDirection)
                return desiredDirection.normalized;

            return Vector2.right;
        }
        
        private Vector2 SelectAxisSlideDirection(bool isNearVerticalBoundary, bool isNearHorizontalBoundary,
            Vector2 fallbackDirection, Vector2 desiredDirection)
        {
            bool slideAlongY;

            if (isNearVerticalBoundary && isNearHorizontalBoundary)
                slideAlongY = Mathf.Abs(fallbackDirection.y) >= Mathf.Abs(fallbackDirection.x);
            else
                slideAlongY = isNearVerticalBoundary;

            if (slideAlongY)
            {
                float sign = GetSign(fallbackDirection.y, desiredDirection.y);
                return new Vector2(0f, sign);
            }
            else
            {
                float sign = GetSign(fallbackDirection.x, desiredDirection.x);
                return new Vector2(sign, 0f);
            }
        }
        
        private float GetSign(float primaryAxisComponent, float fallbackAxisComponent)
        {
            if (Mathf.Abs(primaryAxisComponent) < AxisComponentEpsilon)
                return Mathf.Sign(fallbackAxisComponent == 0f ? 1f : fallbackAxisComponent);

            return Mathf.Sign(primaryAxisComponent);
        }
    }
}