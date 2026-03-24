using System;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    [RequireComponent(typeof(CapsuleCollider2D),typeof(EnemyBrain), typeof(EnemyMovement))]
    public class Enemy : Unit, ISpawnable<Enemy>
    {
        private CapsuleCollider2D _collider;
        private EnemyConfig _config;
        private EnemyBrain _brain;
        private EnemyMovement _movement;
        
        private Vector2 _targetDirection;
        
        public event Action<Enemy> ReadyToSpawn;

        [Inject]
        internal void Initialize(EnemyConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            
            InitializeBase(_config.Size);
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
            Absorbable.Absorbed += OnAbsorbed;
        }
        
        private void Start()
        {
            _brain.Initialize(transform, _collider, _config.BrainData);
            _movement.Initialize(_config.MovementSpeed, Size.Min);
        }

        private void Update()
        {
            _targetDirection = _brain.GetBestTarget(CurrentPosition, Size.Current);
        }

        private void FixedUpdate()
        {
            _movement.MoveByDirection(_targetDirection, Size.Current);
        }

        protected override void Unsubscribe()
        {
            base.Unsubscribe();
            Absorbable.Absorbed -= OnAbsorbed;
        }

        private void OnAbsorbed(IAbsorbable absorbable)
        {
            ReadyToSpawn?.Invoke(this);
        }
    }
}