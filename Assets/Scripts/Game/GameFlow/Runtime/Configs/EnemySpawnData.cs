using System;
using Game.Gameplay;
using UnityEngine;

namespace Game.GameFlow
{
    [Serializable]
    public class EnemySpawnData
    {
        [SerializeField] private EnemyConfig _config;
        [SerializeField] private int _count;
        
        public EnemyConfig Config => _config;
        public int Count => _count;
        
        public void Validate()
        {
            if (_config == null)
                throw new ArgumentNullException(nameof(_config));

            if (_count <= 0)
                throw new ArgumentOutOfRangeException(nameof(_count));

            _config.Validate();
        }
    }
}