using System;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public class SizeData
    {
        [SerializeField, Min(1)] private int _minSize;
        [SerializeField, Min(1)] private int _maxSize;
        
        [SerializeField, Min(0.1f)]private float _delayInDecrease;
        
        public int MinSize => _minSize;
        public int MaxSize => _maxSize;
        
        public float DelayInDecrease => _delayInDecrease;
        
        public void Validate()
        {
            if (_minSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(_minSize));
            
            if (_maxSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(_maxSize));
            
            if (_maxSize < _minSize)
                throw new ArgumentException($"{nameof(_maxSize)} must be greater than or equal to {nameof(_minSize)}");
            
            if (_delayInDecrease <= 0f)
                throw new ArgumentOutOfRangeException(nameof(_delayInDecrease));
        }
    }
}