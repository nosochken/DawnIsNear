using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(Body), typeof(Absorbable))]
    public class Food : MonoBehaviour, ISpawnable<Food>
    {
        private Body _body;
        private Absorbable _absorbable;
        
        public event Action<Food> ReadyToSpawn;

        private void Awake()
        {
            Size size = new Size(1);
            
            _body = GetComponent<Body>();
            _body.Initialize(size);
            
            _absorbable = GetComponent<Absorbable>();
            _absorbable.Initialize(EntityType.Food, _body, size);
        }

        private void OnEnable()
        {
            _absorbable.Absorbed += OnAbsorbed;
        }

        private void OnDisable()
        {
            _absorbable.Absorbed -= OnAbsorbed;
        }

        private void OnAbsorbed(IAbsorbable absorbable)
        {
            ReadyToSpawn?.Invoke(this);
        }
    }
}