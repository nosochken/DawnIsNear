using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayField : MonoBehaviour
    {
        private BoxCollider2D _collider;
        
        private Vector2 _indent = new Vector2(0.07f, 0.22f);
        
        public Vector2 MinPoint => (Vector2)_collider.bounds.min + _indent;
        public Vector2 MaxPoint => (Vector2)_collider.bounds.max - _indent;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
        }

        public Vector2 GetRandomPoint()
        {
            Vector2 min = MinPoint + Vector2.one;
            Vector2 max = MaxPoint - Vector2.one;
            
            float randomX = Random.Range(min.x, max.x);
            float randomY = Random.Range(min.y, max.y);

            return new Vector2(randomX, randomY);
        }
    }
}