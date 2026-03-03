using UnityEngine;

namespace Game.Gameplay
{
    [CreateAssetMenu(menuName = "Configs/Player", fileName = "PlayerConfig")]
    public class PlayerConfig : EntityConfigs
    {
        [SerializeField] private Player _prefab;
        [SerializeField] private MovementDistanceData _distanceData;
        
        public Player Prefab  => _prefab;
        public MovementDistanceData DistanceData => _distanceData;
    }
}