using System;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public class SizeData
    {
        [SerializeField, Range(1, 1)] private int _minSize;
        [SerializeField, Min(1)] private int _maxSize;
        
        [SerializeField, Min(0.1f)]private float _delayInDecrease;
        
        public int MinSize => _minSize;
        public int MaxSize => _maxSize;
        
        public float DelayInDecrease => _delayInDecrease;
    }
}