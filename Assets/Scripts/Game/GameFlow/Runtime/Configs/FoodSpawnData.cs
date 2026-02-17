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
    }
}