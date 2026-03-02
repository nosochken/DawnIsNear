using UnityEngine;

namespace Game.Gameplay
{
    internal sealed class BoundarySteering
    {
        private const float AxisComponentEpsilon = 0.0001f;

        private readonly Collider2D _collider;
        private readonly PlayField _playField;

        private readonly float _avoidDistance;
        private readonly float _epsilon;
        private readonly float _boundaryThreshold;

        internal BoundarySteering(Collider2D collider, PlayField playField,
            float avoidDistance, float epsilon, float boundaryThreshold)
        {
            _collider = collider;
            _playField = playField;

            _avoidDistance = avoidDistance;
            _epsilon = epsilon;
            _boundaryThreshold = boundaryThreshold;
        }

        public Vector2 ComputeBoundaryRepulsion(Vector2 selfPosition)
        {
            //if (_playField == null || _collider == null)
                //return Vector2.zero;

            GetAllowedBounds(out float minAllowedX, out float maxAllowedX, 
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

        public Vector2 ResolveDirection(Vector2 desiredDirection, Vector2 selfPosition, Vector2 threatForce)
        {
            //if (_playField == null || _collider == null)
                //return GetNormalizeOrDefault(desiredDirection, Vector2.right);
            
            GetAllowedBounds(out float minAllowedX, out float maxAllowedX, 
                out float minAllowedY, out float maxAllowedY);

            Vector2 adjustedDirection = desiredDirection;

            bool nearMinX = selfPosition.x <= minAllowedX + _boundaryThreshold;
            bool nearMaxX = selfPosition.x >= maxAllowedX - _boundaryThreshold;
            bool nearMinY = selfPosition.y <= minAllowedY + _boundaryThreshold;
            bool nearMaxY = selfPosition.y >= maxAllowedY - _boundaryThreshold;
            
            if ((nearMinX && adjustedDirection.x < 0f) || (nearMaxX && adjustedDirection.x > 0f))
                adjustedDirection.x = 0f;

            if ((nearMinY && adjustedDirection.y < 0f) || (nearMaxY && adjustedDirection.y > 0f))
                adjustedDirection.y = 0f;

            if (adjustedDirection.sqrMagnitude > SteeringMath.MinSqrMagnitudeForDirection)
                return adjustedDirection.normalized;
            
            bool isNearVerticalBoundary = nearMinX || nearMaxX;
            bool isNearHorizontalBoundary = nearMinY || nearMaxY;

            Vector2 fallbackDirection = SelectFallbackDirection(threatForce, desiredDirection);

            return SelectAxisSlideDirection(isNearVerticalBoundary, isNearHorizontalBoundary, fallbackDirection,
                desiredDirection);
        }
        
        private void GetAllowedBounds(out float minAllowedX, out float maxAllowedX, out float minAllowedY, out float maxAllowedY)
        {
            Vector2 extents = _collider.bounds.extents;
            Vector2 min = _playField.MinPoint;
            Vector2 max = _playField.MaxPoint;

            minAllowedX = min.x + extents.x;
            maxAllowedX = max.x - extents.x;
            minAllowedY = min.y + extents.y;
            maxAllowedY = max.y - extents.y;
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
            if (threatForce.sqrMagnitude > SteeringMath.MinSqrMagnitudeForDirection)
                return threatForce.normalized;

            if (desiredDirection.sqrMagnitude > SteeringMath.MinSqrMagnitudeForDirection)
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