using System;
using UnityEngine;

namespace Game.Gameplay
{
    [CreateAssetMenu(menuName = "Configs/Player", fileName = "PlayerConfig")]
    public class PlayerConfig : EntityConfig
    {
        [SerializeField] private Player _prefab;
        [SerializeField] private MovementDistanceData _distanceData;
        
        public Player Prefab  => _prefab;
        public MovementDistanceData DistanceData => _distanceData;

        protected override void ValidateAdditional()
        {
            if (_prefab == null)
                throw new ArgumentNullException(nameof(_prefab));

            if (_distanceData == null)
                throw new ArgumentNullException(nameof(_distanceData));

            _distanceData.Validate();
        }
    }
}