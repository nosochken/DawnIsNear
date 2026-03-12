using System;
using System.Collections.Generic;
using Game.Gameplay;
using UnityEngine;
using Zenject;

namespace Game.GameFlow
{
    internal class LevelController : MonoBehaviour
    {
        private Player _player;
        private List<EnemySpawner> _enemySpawners;
        private Spawner<Food> _foodSpawner;
        
        private bool _isRunning;
        
        public event Action Won;
        public event Action Lost;

        [Inject]
        private void Construct(Player player)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
        }
        
        internal void Initialize(List<EnemySpawner> enemySpawners, Spawner<Food> foodSpawner)
        {
            if (enemySpawners == null)
                throw new ArgumentNullException(nameof(enemySpawners));

            if (enemySpawners.Count == 0)
                throw new ArgumentException("Enemy spawners collection is empty.", nameof(enemySpawners));

            if (foodSpawner == null)
                throw new ArgumentNullException(nameof(foodSpawner));

            _enemySpawners = enemySpawners;
            _foodSpawner = foodSpawner;
        }

        internal void StartLevel()
        {
            if (_isRunning)
                return;
            
            if (_enemySpawners == null)
                throw new InvalidOperationException("LevelController is not initialized.");

            if (_foodSpawner == null)
                throw new InvalidOperationException("LevelController is not initialized.");
            
            _isRunning = true;
            
            Subscribe();
            
            _foodSpawner.MaintainTargetCount();

            foreach (EnemySpawner enemySpawner in _enemySpawners)
                enemySpawner.SpawnTargetCount();
        }
        
        private void Subscribe()
        {
            _player.Absorbed += OnPlayerAbsorbed;
            
            foreach (EnemySpawner enemySpawner in _enemySpawners)
                enemySpawner.ActiveCountDecreased += OnActiveEnemyCountDecreased;
        }

        internal void StopLevel()
        {
            if (!_isRunning)
                return;
            
            _isRunning = false;
            
            Unsubscribe();
            
            _foodSpawner.StopSpawn();
            
            foreach (EnemySpawner enemySpawner  in _enemySpawners)
                enemySpawner.StopSpawn();
        }

        private void Unsubscribe()
        {
            _player.Absorbed -= OnPlayerAbsorbed;

            foreach (EnemySpawner enemySpawner in _enemySpawners)
                enemySpawner.ActiveCountDecreased -= OnActiveEnemyCountDecreased;
        }
        
        private void OnPlayerAbsorbed(IAbsorbable absorbable)
        {
            if (!_isRunning)
                return;

            StopLevel();
            Lost?.Invoke();
        }

        private void OnActiveEnemyCountDecreased()
        {
            if (!_isRunning)
                return;

            if (AreAllEnemiesAbsorbed())
            {
                StopLevel();
                Won?.Invoke();
            }
        }
        
        private bool AreAllEnemiesAbsorbed()
        {
            foreach (EnemySpawner enemySpawner in _enemySpawners)
            {
                if (enemySpawner.ActiveCount > 0)
                    return false;
            }
            
            return true;
        }
    }
}