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

        [SerializeField] private int _playerWinSize;
        
        public PlayerConfig PlayerConfig => _playerConfig;
        public IReadOnlyList<EnemySpawnData> EnemySpawnData => _enemySpawnData;
        public FoodSpawnData FoodSpawnData => _foodSpawnData;

        public int PlayerWinSize => _playerWinSize;
        
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

            if (_playerWinSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(_playerWinSize));
            
            _playerConfig.Validate();

            if (_playerWinSize < _playerConfig.Size.MinSize)
                throw new ArgumentException($"{nameof(_playerWinSize)} must be greater than or equal to player min size");
            
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