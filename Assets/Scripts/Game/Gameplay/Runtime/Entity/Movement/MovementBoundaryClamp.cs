using UnityEngine;

namespace Game.Gameplay
{
    internal class MovementBoundaryClamp
    {
        private CapsuleCollider2D _collider;
        private PlayField _playField;

        public MovementBoundaryClamp(CapsuleCollider2D collider, PlayField playField)
        {
            _collider = collider;
            _playField = playField;
        }

        public void LimitVelocityWithBorder(ref Vector2 velocity, Vector2 position)
        {
            if (_playField == null) 
                return;
            
            Vector2 extents = _collider.bounds.extents;
            Vector2 min = _playField.MinPoint;
            Vector2 max = _playField.MaxPoint;

            if (position.x <= min.x + extents.x && velocity.x < 0f)
                velocity.x = 0f;
            
            if (position.x >= max.x - extents.x && velocity.x > 0f)
                velocity.x = 0f;

            if (position.y <= min.y + extents.y && velocity.y < 0f)
                velocity.y = 0f;
            
            if (position.y >= max.y - extents.y && velocity.y > 0f)
                velocity.y = 0f;
        }

        public void LimitPositionWithBorder(ref Vector2 position)
        {
            if (_playField == null) 
                return;
            
            Vector2 extents = _collider.bounds.extents;
            Vector2 min = _playField.MinPoint;
            Vector2 max = _playField.MaxPoint;
        
            position.x = Mathf.Clamp(position.x, min.x + extents.x, max.x - extents.x);
            position.y = Mathf.Clamp(position.y, min.y + extents.y, max.y - extents.y);
        }
    }
}