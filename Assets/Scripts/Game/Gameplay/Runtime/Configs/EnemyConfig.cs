using UnityEngine;

namespace Game.Gameplay
{
    [CreateAssetMenu(menuName = "Configs/Enemy", fileName = "EnemyConfig")]
    public class EnemyConfig : EntityConfigs
    {
        [SerializeField] private Enemy _prefab;
        [SerializeField] private EnemyBrainData _bainData;
        
        public Enemy Prefab => _prefab;
        public EnemyBrainData BrainData => _bainData;
        
        //[Header("Слизевой след (если нужен этому врагу)")]
        //public bool SlimeTrailEnabled;

        //[Min(0f)] public float SlimeIntervalSeconds = 0.4f;
        //[Min(0f)] public float SlimeLifeSeconds = 6f;

        // потом добавишь сюда поля под рывок, шипы, плевок и т.д.
        // и будешь использовать их только в тех врагах, кому они нужны.
    }
}