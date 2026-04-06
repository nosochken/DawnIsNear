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
            if (_levelConfig == null)
                throw new System.ArgumentNullException(nameof(_levelConfig));

            if (_levelController == null)
                throw new System.ArgumentNullException(nameof(_levelController));

            if (_enemySpawnerPrefab == null)
                throw new System.ArgumentNullException(nameof(_enemySpawnerPrefab));

            if (_foodSpawnerPrefab == null)
                throw new System.ArgumentNullException(nameof(_foodSpawnerPrefab));

            if (_playField == null)
                throw new System.ArgumentNullException(nameof(_playField));
            
            _levelConfig.Validate();
            
            Container.Bind<PlayerConfig>().FromInstance(_levelConfig.PlayerConfig).AsSingle();
            Container.Bind<Player>().FromComponentInNewPrefab(_levelConfig.PlayerConfig.Prefab).AsSingle().NonLazy();
            Container.Bind<IBody>().FromMethod(context => context.Container.Resolve<Player>().Body).AsSingle();
            
            Container.Bind<PlayField>().FromInstance(_playField).AsSingle();
        }
        
        internal LevelController BuildLevel()
        {
            List<EnemySpawner> enemySpawners = ConstructEnemySpawners();

            Spawner<Food> foodSpawner = Container.InstantiatePrefabForComponent<FoodSpawner>(_foodSpawnerPrefab);
            foodSpawner.Initialize(CreateFood, _levelConfig.FoodSpawnData.Count);
            
            _levelController.Initialize(enemySpawners, foodSpawner);

            return _levelController;
        }
        
        private List<EnemySpawner> ConstructEnemySpawners()
        {
            List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
            
            foreach (EnemySpawnData data in _levelConfig.EnemySpawnData)
            {
                EnemySpawner enemySpawner = Container.InstantiatePrefabForComponent<EnemySpawner>(_enemySpawnerPrefab);
                enemySpawner.Initialize(() => CreateEnemy(data.Config.Prefab, data.Config), data.Count);
                enemySpawners.Add(enemySpawner);
            }

            return enemySpawners;
        }

        private Enemy CreateEnemy(Enemy prefab, EnemyConfig config)
        {
            return Container.InstantiatePrefabForComponent<Enemy>(prefab, new object[] { config });
        }

        private Food CreateFood()
        {
            return Container.InstantiatePrefabForComponent<Food>(_levelConfig.FoodSpawnData.Prefab);
        }
    }
}