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
        private void Construct(EnemyConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            
            InitializeBase(EntityType.Enemy, _config.Size);
        }
        
        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider2D>();
            _brain = GetComponent<EnemyBrain>();
            _movement = GetComponent<EnemyMovement>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Absorbable.Absorbed += OnAbsorbed;
        }
        
        private void Start()
        {
            _brain.Initialize(transform, _collider, _config.BrainData);
            _movement.Initialize(_config.MovementSpeed, Body.Size.Min);
        }

        private void Update()
        {
            _targetDirection = _brain.GetBestTarget(Body.CurrentPosition, Body.Size.Current);
        }

        private void FixedUpdate()
        {
            _movement.MoveByDirection(_targetDirection, Body.Size.Current);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Absorbable.Absorbed -= OnAbsorbed;
        }

        private void OnAbsorbed(IAbsorbable absorbable)
        {
            ReadyToSpawn?.Invoke(this);
        }
    }
}