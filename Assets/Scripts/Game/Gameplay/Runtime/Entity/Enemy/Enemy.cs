using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(EnemyMovement))]
    public class Enemy : Absorber, ISpawnable<Enemy>
    {
        private EnemyMovement _movement;
        
        private Vector2 _targetDirection;
        
        public event Action<Enemy> ReadyToSpawn;
        
        public void Construct(EnemyConfig config, PlayField playField)
        {
            ConstructBase(config.Size);
            
            _movement.Construct(config.MovementSpeed, Size, playField);
        }
        
        protected override void GetComponents()
        {
            base.GetComponents();
            _movement = GetComponent<EnemyMovement>();
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