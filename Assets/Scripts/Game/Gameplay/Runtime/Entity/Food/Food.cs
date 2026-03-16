using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class Food : MonoBehaviour, ITargetable, IAbsorbable, ISpawnable<Food>
    {
        public event Action<IAbsorbable> Absorbed;
        public event Action<Food> ReadyToSpawn;

        public bool IsActive => isActiveAndEnabled;
        public int CurrentSize { get; private set; } = 1;
        public Vector2 CurrentPosition => transform.position;
        public ITargetable Owner => this;

        public void BeAbsorbed()
        {
            Absorbed?.Invoke(this);
            ReadyToSpawn?.Invoke(this);
        }
    }
}