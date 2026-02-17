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
    }
}