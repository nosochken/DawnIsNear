using System;
using UnityEngine;

namespace Game.Gameplay
{
    [CreateAssetMenu(menuName = "Configs/Slime", fileName = "SlimeConfig")]
    public class SlimeConfig : ScriptableObject
    {
        [SerializeField] private Slime _prefab;
        [SerializeField] private SizeData _size;
        [SerializeField] private float _delayInAbsorb;
        
        public Slime Prefab => _prefab;
        public SizeData Size => _size;
        public float DelayInAbsorb => _delayInAbsorb;
        
        public void Validate()
        {
            if (_prefab == null)
                throw new ArgumentNullException(nameof(_prefab));
            
            if (_size == null)
                throw new ArgumentNullException(nameof(_size));
            
            if (_delayInAbsorb <= 0)
                throw new ArgumentOutOfRangeException(nameof(_delayInAbsorb));

            _size.Validate();
        }
    }
}