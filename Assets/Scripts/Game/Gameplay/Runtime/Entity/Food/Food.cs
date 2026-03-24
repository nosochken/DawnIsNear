using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class Food : MonoBehaviour, IAbsorbable, ITargetable, ISpawnable<Food>
    {
        private Size _size;
       
        public event Action<IAbsorbable> Absorbed;
        public event Action<Food> ReadyToSpawn;

        public bool IsActive => isActiveAndEnabled;
        public IValuableSize Size => _size;
        public Vector2 CurrentPosition => transform.position;

        private void Awake()
        {
            _size = new Size(1);
        }

        public void BeAbsorbed()
        {
            Absorbed?.Invoke(this);
            ReadyToSpawn?.Invoke(this);
        }
    }
}