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

        protected override void AdditionalCreationSettings(Enemy enemy, PlayField playField)
        {
            enemy.Construct(_config, playField);

            if (enemy.TryGetComponent(out CapsuleCollider2D collider))
            {
                if (enemy.TryGetComponent(out EnemyBrain enemyBrain))
                    enemyBrain.Construct(collider, _config.BrainData, _player, playField);
            }
        }
    }
}