namespace Game.Gameplay
{
    public class EnemySpawner : Spawner<Enemy>
    {
        private EnemyConfig  _config;

        public void InitializeEnemy(EnemyConfig config)
        {
            _config = config;
        }

        protected override void AdditionalCreationSettings(Enemy enemy)
        {
            enemy.Initialize(_config);
        }
    }
}