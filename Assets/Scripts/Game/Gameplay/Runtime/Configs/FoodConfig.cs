using System;
using UnityEngine;

namespace Game.Gameplay
{
    [CreateAssetMenu(menuName = "Configs/Food", fileName = "FoodConfig")]
    public class FoodConfig : ScriptableObject
    {
        [SerializeField] private Food _prefab;
        [SerializeField] private SizeData _size;
        
        public Food Prefab => _prefab;
        public SizeData Size => _size;
        
        public void Validate()
        {
            if (_prefab == null)
                throw new ArgumentNullException(nameof(_prefab));
            
            if (_size == null)
                throw new ArgumentNullException(nameof(_size));

            _size.Validate();
        }
    }
}