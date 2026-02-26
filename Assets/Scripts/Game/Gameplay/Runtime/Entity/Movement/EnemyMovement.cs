using UnityEngine;

namespace Game.Gameplay
{
    internal class EnemyMovement : Movement
    {
        private const float MinSqrMagnitudeForDirection = 0.0001f;
        
        public void MoveByDirection(Vector2 direction, int size)
        {
            if (direction.sqrMagnitude <= MinSqrMagnitudeForDirection)
            {
                Rigidbody.velocity = Vector2.zero;
                return;
            }
            
            float sizeFactor = CalculateSizeFactor(size);
            
            float speed = MaxSpeed * sizeFactor;
            
            Rigidbody.velocity = direction.normalized * speed;
        }
    }
}