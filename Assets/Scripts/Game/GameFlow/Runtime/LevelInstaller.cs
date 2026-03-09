using System.Collections.Generic;
using Game.Gameplay;
using UnityEngine;
using Zenject;

namespace Game.GameFlow
{
    internal sealed class LevelInstaller : MonoInstaller
    {
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private LevelController _levelController;
        [SerializeField] private EnemySpawner _enemySpawnerPrefab;
        [SerializeField] private FoodSpawner _foodSpawnerPrefab;
        [SerializeField] private PlayField _playField;

        public override void InstallBindings()
        {
            Container.Bind<LevelConfig>().FromInstance(_levelConfig).AsSingle();
            
            Container.Bind<PlayerConfig>().FromInstance(_levelConfig.PlayerConfig).AsSingle();
            Container.Bind<Player>().FromComponentInNewPrefab(_levelConfig.PlayerConfig.Prefab).AsSingle().NonLazy();
            Container.Bind<ITargetable>().To<Player>().FromResolve();
            
            Container.Bind<PlayField>().FromInstance(_playField).AsSingle();
        }
        
        internal LevelController BuildLevel()
        {
            Dictionary<EnemyConfig, EnemySpawner> enemySpawners = ConstructEnemySpawners();

            Spawner<Food> foodSpawner = Container.InstantiatePrefabForComponent<FoodSpawner>(_foodSpawnerPrefab);
            foodSpawner.Initialize(_levelConfig.Food.Prefab, _levelConfig.Food.Count);
            
            _levelController.Initialize(enemySpawners, foodSpawner);

            return _levelController;
        }
        
        private Dictionary<EnemyConfig, EnemySpawner> ConstructEnemySpawners()
        {
            Dictionary<EnemyConfig, EnemySpawner> enemySpawners = new Dictionary<EnemyConfig, EnemySpawner>();
            
            foreach (EnemySpawnData data in _levelConfig.Enemies)
            {
                EnemySpawner enemySpawner = Container.InstantiatePrefabForComponent<EnemySpawner>(_enemySpawnerPrefab);
                enemySpawner.Initialize(data.Config.Prefab, data.Count);
                enemySpawner.InitializeEnemy(data.Config);
                enemySpawners.Add(data.Config, enemySpawner);
            }

            return enemySpawners;
        }
    }
}