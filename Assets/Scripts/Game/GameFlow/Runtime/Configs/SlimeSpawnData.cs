using System;
using Game.Gameplay;
using UnityEngine;

namespace Game.GameFlow
{
    [Serializable]
    public class SlimeSpawnData
    {
        [SerializeField] private SlimeConfig _config;
        [SerializeField] private int _count;
        
        public SlimeConfig Config => _config;
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