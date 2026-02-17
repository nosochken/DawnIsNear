using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayField : MonoBehaviour
    {
        private BoxCollider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
        }

        public Vector2 GetRandomPoint()
        {
            Vector2 min = (Vector2)_collider.bounds.min + Vector2.one;
            Vector2 max = (Vector2)_collider.bounds.max - Vector2.one;

            float randomX = Random.Range(min.x, max.x);
            float randomY = Random.Range(min.y, max.y);

            return new Vector2(randomX, randomY);
        }
    }
}