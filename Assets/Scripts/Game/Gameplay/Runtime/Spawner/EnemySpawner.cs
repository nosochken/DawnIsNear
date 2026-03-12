using System;
using Zenject;

namespace Game.Gameplay
{
    public class EnemySpawner : Spawner<Enemy>
    {
        private EnemyConfig  _config;

        public void InitializeEnemy(EnemyConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }
        
        protected override Enemy CreateSpawnable(DiContainer container, Enemy prefab)
        {
            return container.InstantiatePrefabForComponent<Enemy>(prefab, new object[] { _config });
        }
    }
}