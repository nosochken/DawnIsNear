using System.Collections.Generic;
using UnityEngine;
using Game.Gameplay;
using Game.Presentation;

namespace Game.GameFlow
{
    internal class LevelInstaller : MonoBehaviour
    {
        [SerializeField] private LevelConfig _config;
        [SerializeField] private LevelController _levelController;
        [SerializeField] private CameraMovement _cameraMovement;
        [SerializeField] private PlayField _playField;
        [SerializeField] private EnemySpawner _enemySpawnerPrefab;
        [SerializeField] private FoodSpawner _foodSpawnerPrefab;

        public LevelController BuildLevel()
        {
            Player player = Instantiate(_config.PlayerConfig.Prefab);
            player.Construct(_config.PlayerConfig);
            _cameraMovement.Construct(player);

            Dictionary<EnemyConfig, EnemySpawner> enemySpawners = ConstructEnemySpawners(player);

            Spawner<Food> foodSpawner = Instantiate(_foodSpawnerPrefab);
            foodSpawner.Construct(_config.Food.Prefab, _config.Food.Count, _playField);
            
            _levelController.Construct(_config, player, enemySpawners, foodSpawner);

            return _levelController;
        }

        private Dictionary<EnemyConfig, EnemySpawner> ConstructEnemySpawners(ITargetable target)
        {
            Dictionary<EnemyConfig, EnemySpawner> enemySpawners = new Dictionary<EnemyConfig, EnemySpawner>();
            
            foreach (EnemySpawnData data in _config.Enemies)
            {
                EnemySpawner enemySpawner = Instantiate(_enemySpawnerPrefab);
                enemySpawner.Construct(data.Config.Prefab, data.Count, _playField);
                enemySpawner.ConstructEnemyBrain(data.Config,target);
                enemySpawners.Add(data.Config, enemySpawner);
            }

            return enemySpawners;
        }
    }
}