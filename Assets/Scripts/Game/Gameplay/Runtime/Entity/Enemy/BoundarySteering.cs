using UnityEngine;

namespace Game.Gameplay
{
    internal sealed class BoundarySteering
    {
        private const float MinSqrMagnitudeForDirection = 0.0001f;
        private const float AxisComponentEpsilon = 0.0001f;

        private readonly Collider2D _collider;
        private readonly PlayField _playField;

        private readonly float _avoidDistance;
        private readonly float _epsilon;
        private readonly float _boundaryThreshold;

        public BoundarySteering(Collider2D collider, PlayField playField,
            float avoidDistance, float epsilon, float boundaryThreshold)
        {
            _collider = collider;
            _playField = playField;

            _avoidDistance = Mathf.Max(avoidDistance, 0.0001f);
            _epsilon = Mathf.Max(epsilon, 0.000001f);
            _boundaryThreshold = Mathf.Max(boundaryThreshold, 0f);
        }

        public Vector2 ComputeBoundaryRepulsion(Vector2 position)
        {
            //if (_playField == null || _collider == null)
                //return Vector2.zero;

            GetAllowedBounds(out float minAllowedX, out float maxAllowedX, 
                out float minAllowedY, out float maxAllowedY);

            float distanceToLeftBoundary = position.x - minAllowedX;
            float distanceToRightBoundary = maxAllowedX - position.x;
            float distanceToBottomBoundary = position.y - minAllowedY;
            float distanceToTopBoundary = maxAllowedY - position.y;

            Vector2 force = Vector2.zero;

            force += ComputeRepulsionFromBoundary(Vector2.right, distanceToLeftBoundary);
            force += ComputeRepulsionFromBoundary(Vector2.left, distanceToRightBoundary);
            force += ComputeRepulsionFromBoundary(Vector2.up, distanceToBottomBoundary);
            force += ComputeRepulsionFromBoundary(Vector2.down, distanceToTopBoundary);

            return force;
        }

        public Vector2 ResolveDirection(Vector2 desiredDirection, Vector2 position, Vector2 escapeDirection)
        {
            //if (_playField == null || _collider == null)
                //return GetNormalizeOrDefault(desiredDirection, Vector2.right);
            
            GetAllowedBounds(out float minAllowedX, out float maxAllowedX, 
                out float minAllowedY, out float maxAllowedY);

            Vector2 direction = desiredDirection;

            bool nearMinX = position.x <= minAllowedX + _boundaryThreshold;
            bool nearMaxX = position.x >= maxAllowedX - _boundaryThreshold;
            bool nearMinY = position.y <= minAllowedY + _boundaryThreshold;
            bool nearMaxY = position.y >= maxAllowedY - _boundaryThreshold;
            
            if ((nearMinX && direction.x < 0f) || (nearMaxX && direction.x > 0f))
                direction.x = 0f;

            if ((nearMinY && direction.y < 0f) || (nearMaxY && direction.y > 0f))
                direction.y = 0f;

            if (direction.sqrMagnitude > MinSqrMagnitudeForDirection)
                return direction.normalized;
            
            bool isNearVerticalBoundary = nearMinX || nearMaxX;
            bool isNearHorizontalBoundary = nearMinY || nearMaxY;

            Vector2 preferredEscapeDirection =
                GetNormalizeOrDefault(escapeDirection, GetNormalizeOrDefault(desiredDirection, Vector2.right));

            return ChooseAxisSlideDirection(isNearVerticalBoundary, isNearHorizontalBoundary, preferredEscapeDirection,
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
        
        private Vector2 GetNormalizeOrDefault(Vector2 value, Vector2 defaultValue)
        {
            return value.sqrMagnitude > MinSqrMagnitudeForDirection
                ? value.normalized
                : defaultValue;
        }
        
        private Vector2 ChooseAxisSlideDirection(bool isNearVerticalBoundary, bool isNearHorizontalBoundary,
            Vector2 preferredEscapeDirection, Vector2 desiredDirection)
        {
            bool slideAlongY;

            if (isNearVerticalBoundary && isNearHorizontalBoundary)
                slideAlongY = Mathf.Abs(preferredEscapeDirection.y) >= Mathf.Abs(preferredEscapeDirection.x);
            else
                slideAlongY = isNearVerticalBoundary;

            if (slideAlongY)
            {
                float sign = GetSign(preferredEscapeDirection.y, desiredDirection.y);
                return new Vector2(0f, sign);
            }
            else
            {
                float sign = GetSign(preferredEscapeDirection.x, desiredDirection.x);
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