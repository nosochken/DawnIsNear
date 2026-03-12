using System;
using System.Collections.Generic;
using Game.Gameplay;
using UnityEngine;
using Zenject;

namespace Game.GameFlow
{
    internal class LevelController : MonoBehaviour
    {
        private LevelConfig _levelConfig;
        private Player _player;
        private Dictionary<EnemyConfig, EnemySpawner> _enemySpawners;
        private Spawner<Food> _foodSpawner;
        
        private bool _isRunning;
        
        public event Action Won;
        public event Action Lost;

        [Inject]
        private void Construct(LevelConfig config, Player player)
        {
            _levelConfig = config ?? throw new ArgumentNullException(nameof(config));
            _player = player ?? throw new ArgumentNullException(nameof(player));
        }
        
        internal void Initialize(Dictionary<EnemyConfig, EnemySpawner> enemySpawners, Spawner<Food> foodSpawner)
        {
            if (enemySpawners == null)
                throw new ArgumentNullException(nameof(enemySpawners));

            if (enemySpawners.Count == 0)
                throw new ArgumentException("Enemy spawners collection is empty.", nameof(enemySpawners));

            foreach (var pair in enemySpawners)
            {
                if (pair.Key == null)
                    throw new ArgumentException("Enemy config key is null.", nameof(enemySpawners));

                if (pair.Value == null)
                    throw new ArgumentException("Enemy spawner value is null.", nameof(enemySpawners));
            }

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
            
            _foodSpawner.MaintainCount(_levelConfig.FoodSpawnData.Count);
            
            foreach (EnemySpawnData data in _levelConfig.EnemySpawnData)
                _enemySpawners[data.Config].Spawn(data.Count);
        }
        
        private void Subscribe()
        {
            _player.SizeChanged += OnPlayerSizeChanged;
            _player.Absorbed += OnPlayerAbsorbed;
        }

        internal void StopLevel()
        {
            if (!_isRunning)
                return;
            
            _isRunning = false;
            
            Unsubscribe();
            
            _foodSpawner.StopSpawn();
            
            foreach (var enemySpawner  in _enemySpawners)
                enemySpawner.Value.StopSpawn();
        }

        private void Unsubscribe()
        {
            _player.SizeChanged -= OnPlayerSizeChanged;
            _player.Absorbed -= OnPlayerAbsorbed;
        }
        
        private void OnPlayerSizeChanged(int size)
        {
            if (!_isRunning)
                return;
            
            //Debug.Log(size);
            //поменять нужно будет, на что все противники поглощены на уровне

            if (size >= _levelConfig.PlayerWinSize)
            {
                StopLevel();
                Won?.Invoke();
            }
        }
        
        private void OnPlayerAbsorbed(IAbsorbable absorbable)
        {
            if (!_isRunning)
                return;

            StopLevel();
            Lost?.Invoke();
        }
    }
}