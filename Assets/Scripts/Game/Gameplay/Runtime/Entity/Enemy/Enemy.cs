using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Movement))]
    public class Enemy : Absorber, ISpawnable<Enemy>
    {
        private Movement _movement;
        
        private Vector2 _targetDirection;
        
        public event Action<Enemy> ReadyToSpawn;
        
        protected override void ExtendConstructor(EntityConfigs config)
        {
            _movement.Construct(config.Movement, Size);
        }
        
        protected override void GetComponents()
        {
            base.GetComponents();
            _movement = GetComponent<Movement>();
        }
        
        protected override void Subscribe()
        {
            base.Subscribe();
            Absorbed += OnAbsorbed;
        }
        
        private void FixedUpdate()
        {
            _movement.MoveByDirection(_targetDirection, Size);
        }

        protected override void Unsubscribe()
        {
            base.Unsubscribe();
            Absorbed -= OnAbsorbed;
        }
        
        public void SetTargetDirection(Vector2 targetDirection)
        {
            _targetDirection = targetDirection;
        }

        private void OnAbsorbed(IAbsorbable absorbable)
        {
            ReadyToSpawn?.Invoke(this);
        }
    }
}