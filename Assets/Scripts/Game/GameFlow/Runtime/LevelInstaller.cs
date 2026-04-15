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
        [SerializeField] private SlimeSpawner _slimeSpawnerPrefab;
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
            
            if (_slimeSpawnerPrefab == null)
                throw new System.ArgumentNullException(nameof(_slimeSpawnerPrefab));

            if (_playField == null)
                throw new System.ArgumentNullException(nameof(_playField));
            
            _levelConfig.Validate();
            
            Container.Bind<PlayerConfig>().FromInstance(_levelConfig.PlayerConfig).AsSingle();
            Container.Bind<Player>().FromComponentInNewPrefab(_levelConfig.PlayerConfig.Prefab).AsSingle().NonLazy();
            Container.Bind<IBody>().FromMethod(context => context.Container.Resolve<Player>().Body).AsSingle();
            Container.Bind<IAbsorbable>().FromMethod(context => context.Container.Resolve<Player>().Absorbable).AsSingle();
            
            Container.Bind<PlayField>().FromInstance(_playField).AsSingle();
        }
        
        internal LevelController BuildLevel()
        {
            List<EnemySpawner> enemySpawners = ConstructEnemySpawners();

            FoodSpawner foodSpawner = Container.InstantiatePrefabForComponent<FoodSpawner>(_foodSpawnerPrefab);
            foodSpawner.Initialize(() => CreateFood(_levelConfig.FoodSpawnData.Config), _levelConfig.FoodSpawnData.Count);

            SlimeSpawner slimeSpawner = Container.InstantiatePrefabForComponent<SlimeSpawner>(_slimeSpawnerPrefab);
            slimeSpawner.Initialize(() => CreateSlime(_levelConfig.SlimeSpawnData.Config), _levelConfig.SlimeSpawnData.Count);
            
            _levelController.Initialize(enemySpawners, foodSpawner, slimeSpawner);

            return _levelController;
        }
        
        private List<EnemySpawner> ConstructEnemySpawners()
        {
            List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
            
            foreach (EnemySpawnData data in _levelConfig.EnemySpawnData)
            {
                EnemySpawner enemySpawner = Container.InstantiatePrefabForComponent<EnemySpawner>(_enemySpawnerPrefab);
                enemySpawner.Initialize(() => CreateEnemy(data.Config), data.Count);
                enemySpawners.Add(enemySpawner);
            }

            return enemySpawners;
        }

        private Enemy CreateEnemy(EnemyConfig config)
        {
            return Container.InstantiatePrefabForComponent<Enemy>(config.Prefab, new object[] { config });
        }

        private Food CreateFood(FoodConfig config)
        {
            return Container.InstantiatePrefabForComponent<Food>(config.Prefab, new object[] { config });
        }
        
        private Slime CreateSlime(SlimeConfig config)
        {
            return Container.InstantiatePrefabForComponent<Slime>(config.Prefab, new object[] { config });
        }
    }
}