using UnityEngine;

namespace Game.Gameplay
{
    public class EnemySpawner : Spawner<Enemy>
    {
        private EnemyConfig  _config;
        private ITargetable _player;

        public void ConstructEnemyBrain(EnemyConfig config, ITargetable player)
        {
            _config = config;
            _player = player;
        }

        protected override void AdditionalCreationSettings(Enemy enemy)
        {
            enemy.Construct(_config);

            if (enemy.TryGetComponent(out EnemyBrain enemyBrain))
                enemyBrain.Construct(_player, _config.BrainData);
        }
    }
}