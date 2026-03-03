using UnityEngine;

namespace  Game.Gameplay
{
    public abstract class EntityConfigs : ScriptableObject
    {
        [SerializeField] private SizeData _size;
        [SerializeField] private MovementSpeedData _movementSpeed;
        
        public SizeData Size => _size;
        public MovementSpeedData MovementSpeed => _movementSpeed;
    }
}