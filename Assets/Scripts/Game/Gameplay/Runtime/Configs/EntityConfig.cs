using System;
using UnityEngine;

namespace  Game.Gameplay
{
    public abstract class EntityConfig : ScriptableObject
    {
        [SerializeField] private SizeData _size;
        [SerializeField] private MovementSpeedData _movementSpeed;
        
        public SizeData Size => _size;
        public MovementSpeedData MovementSpeed => _movementSpeed;
        
        public void Validate()
        {
            if (_size == null)
                throw new ArgumentNullException(nameof(_size));

            if (_movementSpeed == null)
                throw new ArgumentNullException(nameof(_movementSpeed));

            _size.Validate();
            _movementSpeed.Validate();
            
            ValidateAdditional();
        }
        
        protected abstract void ValidateAdditional();
    }
}