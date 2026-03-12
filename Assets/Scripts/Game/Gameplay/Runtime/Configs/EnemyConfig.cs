using System;
using UnityEngine;

namespace Game.Gameplay
{
    [CreateAssetMenu(menuName = "Configs/Enemy", fileName = "EnemyConfig")]
    public class EnemyConfig : EntityConfig
    {
        [SerializeField] private Enemy _prefab;
        [SerializeField] private EnemyBrainData _brainData;
        
        public Enemy Prefab => _prefab;
        public EnemyBrainData BrainData => _brainData;

        protected override void ValidateAdditional()
        {
            if (_prefab == null)
                throw new ArgumentNullException(nameof(_prefab));

            if (_brainData == null)
                throw new ArgumentNullException(nameof(_brainData));

            _brainData.Validate();
        }
    }
}