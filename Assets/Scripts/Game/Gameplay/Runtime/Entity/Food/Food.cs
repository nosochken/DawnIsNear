using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class Food : MonoBehaviour, IAbsorbable, ISpawnable<Food>
    {
        public event Action<IAbsorbable> Absorbed;
        public event Action<Food> ReadyToSpawn;

        public int Size { get; private set; } = 1;
        public Vector2 CurrentPosition => transform.position;
        
        public bool IsActive => isActiveAndEnabled;

        public void BeAbsorbed()
        {
            Absorbed?.Invoke(this);
            ReadyToSpawn?.Invoke(this);
        }
    }
}