using System;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(EnemyBrain), typeof(EnemyMovement))]
    public class Enemy : Absorber, ISpawnable<Enemy>
    {
        private CapsuleCollider2D _collider;
        private EnemyBrain _brain;
        private EnemyMovement _movement;
        
        private Vector2 _targetDirection;
        
        public event Action<Enemy> ReadyToSpawn;

        internal void Initialize(EnemyConfig config)
        {
            InitializeBase(config.Size);
            _brain.Initialize(transform, _collider, config.BrainData);
            _movement.Initialize(config.MovementSpeed, Size);
        }
        
        protected override void GetComponents()
        {
            base.GetComponents();
            _collider = GetComponent<CapsuleCollider2D>();
            _brain = GetComponent<EnemyBrain>();
            _movement = GetComponent<EnemyMovement>();
        }
        
        protected override void Subscribe()
        {
            base.Subscribe();
            Absorbed += OnAbsorbed;
        }

        private void Update()
        {
            _targetDirection = _brain.GetBestTarget(CurrentPosition, Size);
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