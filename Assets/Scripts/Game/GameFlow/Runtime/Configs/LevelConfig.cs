using System.Collections.Generic;
using Game.Gameplay;
using UnityEngine;

namespace Game.GameFlow
{
    [CreateAssetMenu(menuName =  "Configs/Level", fileName = "LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private EnemySpawnData[] _enemies;
        [SerializeField] private FoodSpawnData _food;

        [SerializeField] private int _playerWinSize;
        
        public PlayerConfig PlayerConfig => _playerConfig;
        public IReadOnlyList<EnemySpawnData> Enemies => _enemies;
        public FoodSpawnData Food => _food;

        public int PlayerWinSize => _playerWinSize;
    }
}