using UnityEngine;

namespace Game.Gameplay
{
    internal class EnemyMovement : Movement
    {
        internal void MoveByDirection(Vector2 direction, int size)
        {
            if (direction.sqrMagnitude <= DirectionMath.MinSqrMagnitudeForDirection)
            {
                Stop();
                return;
            }
            
            float sizeFactor = CalculateSizeFactor(size);
            float speed = MaxSpeed * sizeFactor;
            
            Vector2 velocity = direction.normalized * speed;
            Move(GetNextPosition(velocity));
        }
    }
}