using UnityEngine;

namespace Game.Gameplay
{
    internal sealed class MovementBoundary
    {
        private readonly CapsuleCollider2D _collider;
        private readonly PlayField _playField;
        
        internal MovementBoundary(CapsuleCollider2D  collider, PlayField playField)
        {
            _collider = collider;
            _playField = playField;
        }

        internal void BlockOutboundAxes(ref Vector2 direction, Vector2 position, float threshold = 0f)
        {
            GetNearFlags(out bool nearMinX, out bool nearMaxX, out bool nearMinY,
                out bool nearMaxY, position, threshold);

            if ((nearMinX && direction.x < 0f) || (nearMaxX && direction.x > 0f))
                direction.x = 0f;

            if ((nearMinY && direction.y < 0f) || (nearMaxY && direction.y > 0f))
                direction.y = 0f;
        }
        
        internal void GetNearFlags(out bool nearMinX, out bool nearMaxX, out bool nearMinY, out bool nearMaxY,
            Vector2 position, float threshold = 0f)
        {
            GetAllowedBounds(out float minX, out float maxX, out float minY, out float maxY);

            nearMinX = position.x <= minX + threshold;
            nearMaxX = position.x >= maxX - threshold;
            nearMinY = position.y <= minY + threshold;
            nearMaxY = position.y >= maxY - threshold;
        }

        internal void ClampPosition(ref Vector2 position)
        {
            GetAllowedBounds(out float minX, out float maxX, out float minY, out float maxY);

            position.x = Mathf.Clamp(position.x, minX, maxX);
            position.y = Mathf.Clamp(position.y, minY, maxY);
        }
        
        internal void GetAllowedBounds(out float minX, out float maxX,
            out float minY, out float maxY)
        {
            Vector2 extents = _collider.bounds.extents;
            Vector2 min = _playField.MinPoint;
            Vector2 max = _playField.MaxPoint;

            minX = min.x + extents.x;
            maxX = max.x - extents.x;
            minY = min.y + extents.y;
            maxY = max.y - extents.y;
        }
    }
}