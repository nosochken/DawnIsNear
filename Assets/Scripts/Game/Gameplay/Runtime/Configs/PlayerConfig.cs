using UnityEngine;

namespace Game.Gameplay
{
    [CreateAssetMenu(menuName = "Configs/Player", fileName = "PlayerConfig")]
    public class PlayerConfig : EntityConfigs
    {
        [SerializeField] private Player _prefab;
        
        public Player Prefab  => _prefab;
    }
}