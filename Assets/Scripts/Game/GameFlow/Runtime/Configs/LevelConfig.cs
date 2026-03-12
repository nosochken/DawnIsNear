using System;
using System.Collections.Generic;
using Game.Gameplay;
using UnityEngine;

namespace Game.GameFlow
{
    [CreateAssetMenu(menuName =  "Configs/Level", fileName = "LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private EnemySpawnData[] _enemySpawnData;
        [SerializeField] private FoodSpawnData _foodSpawnData;
        
        public PlayerConfig PlayerConfig => _playerConfig;
        public IReadOnlyList<EnemySpawnData> EnemySpawnData => _enemySpawnData;
        public FoodSpawnData FoodSpawnData => _foodSpawnData;
        
        public void Validate()
        {
            if (_playerConfig == null)
                throw new ArgumentNullException(nameof(_playerConfig));

            if (_enemySpawnData == null)
                throw new ArgumentNullException(nameof(_enemySpawnData));

            if (_enemySpawnData.Length == 0)
                throw new ArgumentException($"{nameof(_enemySpawnData)} must contain at least one element");

            if (_foodSpawnData == null)
                throw new ArgumentNullException(nameof(_foodSpawnData));
            
            _playerConfig.Validate();
            _foodSpawnData.Validate();

            for (int i = 0; i < _enemySpawnData.Length; i++)
            {
                if (_enemySpawnData[i] == null)
                    throw new ArgumentNullException($"{nameof(_enemySpawnData)}[{i}]");

                _enemySpawnData[i].Validate();
            }
        }
    }
}