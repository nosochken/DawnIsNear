using System;
using Game.Gameplay;
using UnityEngine;

namespace Game.GameFlow
{
    [Serializable]
    public class FoodSpawnData
    {
        [SerializeField] private Food _prefab;
        [SerializeField] private int _count;
        
        public Food Prefab => _prefab;
        public int Count => _count;
        
        public void Validate()
        {
            if (_prefab == null)
                throw new ArgumentNullException(nameof(_prefab));

            if (_count <= 0)
                throw new ArgumentOutOfRangeException(nameof(_count));
        }
    }
}