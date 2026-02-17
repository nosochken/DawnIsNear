using System;
using System.Collections.Generic;
using Game.Gameplay;
using UnityEngine;

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

        public void Construct(LevelConfig config, Player player, 
            Dictionary<EnemyConfig, EnemySpawner> enemySpawners, Spawner<Food> foodSpawner)
        {
            _levelConfig = config;
            _player = player;
            _enemySpawners = enemySpawners;
            _foodSpawner = foodSpawner;
        }

        public void StartLevel()
        {
            if (_isRunning)
                return;
            
            _isRunning = true;
            
            Subscribe();
            
            _foodSpawner.MaintainCount(_levelConfig.Food.Count);
            
            foreach (EnemySpawnData data in _levelConfig.Enemies)
                _enemySpawners[data.Config].Spawn(data.Count);
        }
        
        private void Subscribe()
        {
            _player.SizeChanged += OnPlayerSizeChanged;
            _player.Absorbed += OnPlayerAbsorbed;
        }

        public void StopLevel()
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
            
            Debug.Log(size);
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